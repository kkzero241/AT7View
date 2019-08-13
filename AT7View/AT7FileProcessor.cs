using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AT7View
{
    class AT7FileProcessor
    {
        //private static List<byte> inputFile;
        //private static List<byte> newFile;
        private static string header; //Used as the header check in the decompressor
        private static byte flagByte; //Byte of the compression flag for the next 8-16 bytes in a compressed archive
        private static string flagBits; //The above stored as a string of bits, used in counting which bytes are raw and which are compressed
        private static string fileName; //Pretty self explanatory
        private static UInt16 blockSize; //Size of the current AT7P block in the decompressor
        //private static int indexOnList;
        private static byte[] fourBytes; //Used to grab the header's bytes in the decompressor
        private static int backtrack; //Stores how far away a duplicate data string is from the current position in the decompressor
        private static byte[] swapper; //Used in the decompressor
        private static byte controlNybble; //The top nybble of a 2-byte code in the compressed data
        private static byte[] ccodeStuff; //Handles 2-byte control codes in compressed data
        private static int count; //Position of the recompressor in the entire raw archive
        private static List<byte> toRepack; //The raw file being used for recompression
        private static List<byte> inputList = new List<byte>(); //Used for finding duplicate data in recompression
        private static readonly byte[] at7pHeader = { 0x41, 0x54, 0x37, 0x50 }; //AT7P header, beginning of compressed block
        private static readonly byte[] at7eHeader = { 0x41, 0x54, 0x37, 0x45 }; //AT7E header, end of file
        private static int rawPos; //Position of the recompressor within a chunk of 0xC000 bytes
        //private static List<byte> toRepack;



        private static string ByteToBitsString(byte byteIn)
        {

            string theValue = "";
            theValue = ((byteIn / 128) % 2).ToString() +
            ((byteIn / 64) % 2) +
            ((byteIn / 32) % 2) +
            ((byteIn / 16) % 2) +
            ((byteIn / 8) % 2) +
            ((byteIn / 4) % 2) +
            ((byteIn / 2) % 2) +
            ((byteIn / 1) % 2);
            return theValue;
        }

        /*public static string getFileName()
        {
            return fileName;
        }*/

        private static int NoNegativeCount(int val) 
        {
            if(val > count)
            {
                return count;
            }
            else
            {
                return val;
            }
            
        }

        private static int NoOOBCount(int val, long len) //Makes sure the search doesn't throw an out of bounds
        {
            if(count + val > len)
            {
                //Console.WriteLine("Condensed length is " + (val - ((count + val) - (int)len)));
                return val - ((count + val) - (int)len);
            }
            else
            {
                return val;
            }
        }

        private static int NoNegativeSearchPos() //Determines which position to start at in finding duplicate data
        {
            if (count < 0x1000)
            {
                return 0;
            }
            else
            {
                return count - 0xFFF;
            }
        }

        private static string AddZeroes(string toAppend, int length) //Adds zeroes at the leftmost part of the string
        {
            while (toAppend.Length < length)
            {
                //Console.WriteLine("codeToInsert is now length " + codeToInsert.Length);
                toAppend = toAppend.Insert(0, "0");
            }
            return toAppend;
        }
       
        private static string AddZeroesReverse(string toAppend, int length) //Adds zeroes at the rightmost part of the string
        {
            while (toAppend.Length < length)
            {
                
                //Console.WriteLine("codeToInsert is now length " + codeToInsert.Length);
                toAppend = toAppend.Insert(toAppend.Length, "0");
            }
            return toAppend;
        }

        private static int[] FindDupe(List<byte> input, List<byte> toRepack)
        {
            /*
                         * 8-16 BYTE STRING CHECKING PROCEDURE
                         * Do a for loop from 18 subtracted down to equal 3
                         * In each pass, check if the string starting from count
                         is in currentBlock within a range of 0x1000 behind count
                         * If so, nab the control nybble and 12-bit negative offset,
                         make a 2-byte value, and add it to currentString, incrementing
                         count by how far away the duplicate section was
                         * If not, simply add the byte in toRepack at count
                         and increment count by 1
                        */
            
            int noNegSearch = NoNegativeSearchPos(); //Where to start in searching for a dupe
            int[] toReturn = new int[2]; //Index 0 is the dupe's position, index 1 is the dupe's length
            toReturn[0] = -1;
            toReturn[1] = -1;
            //int searchPos;
            if (count > 3)
            {
                for (int strLength = NoOOBCount(NoNegativeCount(18), (long)toRepack.Count); strLength >= 3; strLength--)
                {
                    //Console.WriteLine("strLength is " + strLength);
                    //!Enumerable.SequenceEqual
                    //Console.WriteLine("rawPos div/trunc'd equals " + (int)Math.Truncate((double)rawPos / 4));
                    for (int searchPos = noNegSearch; searchPos < count - strLength; searchPos += (rawPos / 200) + 1) //If I did searchPos++ then recompression would be like 15 hours lmao
                    {
                        inputList = input.GetRange(0, strLength);
                        //Console.WriteLine(toRepack.GetRange(searchPos, strLength));
                        //Console.WriteLine(input.GetRange(0, strLength));
                        if (Enumerable.SequenceEqual(toRepack.GetRange(searchPos, strLength), inputList))
                        {
                            //Console.WriteLine(string.Join(",", toRepack.GetRange(searchPos, strLength)));
                            //Console.WriteLine(string.Join(",", input.GetRange(0, strLength)));
                            toReturn[0] = searchPos;
                            toReturn[1] = strLength;
                            //Console.WriteLine(toReturn[0] + " " + toReturn[1]);
                            return toReturn;
                        }

                    }
                    /*searchPos = noNegSearch;
                    try
                    {
                        while (Enumerable.SequenceEqual(toRepack.GetRange(searchPos, strLength), inputList))
                        {

                            Console.WriteLine(string.Join(",", toRepack.GetRange(searchPos, strLength)));
                            Console.WriteLine(string.Join(",", input.GetRange(0, strLength)));
                            toReturn[0] = searchPos;
                            toReturn[1] = strLength;
                            searchPos++;


                        }
                    }
                    catch (System.ArgumentException)
                    {
                        break;
                    }*/

                }
            }
            
            return toReturn;
            
        }

        public static void decompressAT7P(BinaryReader theStuff, string theFilename)
        {
            BinaryWriter theDecomp = new BinaryWriter(File.Open($"{theFilename}.raw", FileMode.Create));
            //inputFile = new List<byte>();
            //newFile = new List<byte>();
            List<byte> rawData = new List<byte>();
            List<byte> toAdd = new List<byte>();
            fileName = theFilename;
            theStuff.BaseStream.Position = 0;
            long length = theStuff.BaseStream.Length;
            //inputFile.AddRange(theFile.ReadBytes((int)theFile.BaseStream.Length));
            //indexOnList = 0;
            /*while (theFile.BaseStream.Position < theFile.BaseStream.Length)
            {
                //First we get the header--AT7P, AT7X, or AT7E
                header = System.Text.Encoding.UTF8.GetString(theFile.ReadBytes(4));
                header.Reverse();
                Console.WriteLine($"{header}, pass {count}");
                /*fourBytes = new byte[4];
                fourBytes[0] = inputFile[indexOnList];
                fourBytes[1] = inputFile[indexOnList + 1];
                fourBytes[2] = inputFile[indexOnList + 2];
                fourBytes[3] = inputFile[indexOnList + 3];
                Console.WriteLine(inputFile[0] + " " + inputFile[1] + " " + inputFile[2] + " " + inputFile[3]);
                count++;

                
                //For AT7P
                if(header.Equals("AT7P"))
                {
                    //indexOnList += 4;
                    
                    blockSize = BitConverter.ToUInt16(theFile.ReadBytes(2).Reverse().ToArray(), 0);
                    //indexOnList += 2;

                    while((int)theFile.BaseStream.Position < blockSize)
                    {
                        flagByte = theFile.ReadByte();
                        flagBits = ByteToBitsString(flagByte);

                        foreach (char theBit in flagBits)
                        {
                            Console.WriteLine(theBit);
                            if (theBit.Equals('1') && theFile.BaseStream.Position < theFile.BaseStream.Length)
                            {
                                newFile.Add(theFile.ReadByte());
                            }
                            else if (theBit.Equals('0') && theFile.BaseStream.Position < theFile.BaseStream.Length)
                            {
                                swapper = new byte[2];
                                swapper = theFile.ReadBytes(2).Reverse().ToArray();
                                Console.WriteLine("Swapper: " + swapper[0] + " and " + swapper[1]);
                                //newFile.Add(swapper[0]); newFile.Add(swapper[1]);
                                controlNybble = (byte)((swapper[1] >> 4) & 0x0F); //The control code
                                ccodeStuff = new byte[2];
                                ccodeStuff[1] = (byte)(swapper[1] & 0xF); //The 12-bit negative offset
                                ccodeStuff[0] = swapper[0];
                                Console.WriteLine(BitConverter.ToInt16(ccodeStuff, 0));
                                backtrack = 0x1000 - BitConverter.ToInt16(ccodeStuff, 0);
                                for (int n = 0; n < 3 + controlNybble; n++)
                                {
                                    try
                                    {
                                        newFile.Add(newFile.ElementAt((newFile.Count() - backtrack) + n));
                                    }
                                    catch (System.ArgumentOutOfRangeException)
                                    {
                                        Console.WriteLine("Caught a snazzy out of range.");
                                    }
                                }
                            }
                        }

                    }

                    //indexOnList = blockSize;

                }

                //For AT7X
                else if (header.Equals("AT7X"))
                {
                    theFile.ReadBytes(6);
                    newFile.AddRange(theFile.ReadBytes(0xC000).Reverse());
                }
                //For AT7E--we don't need this in the uncompressed archive so we just end this part of the process
                else if (header.Equals("AT7E"))
                {
                    break;
                }

                count++;*/
            //}

            while (theStuff.BaseStream.Position < length)
            {

                long blockStart = theStuff.BaseStream.Position;
                fourBytes = new byte[4];

                toAdd = new List<byte>();
                for (int i = 0; i < 4; i++)
                {
                    fourBytes[i] = (byte)theStuff.ReadByte();

                }
                header = System.Text.Encoding.UTF8.GetString(fourBytes);

                if (header.Equals("AT7P"))
                {
                    /*rawData.Add(0x41);
                    rawData.Add(0x54);
                    rawData.Add(0x37);
                    rawData.Add(0x58);
                    rawData.Add(0x00);
                    rawData.Add(0xC0);*/
                    Console.WriteLine("Header detected, valid AT7P block");
                    blockSize = BitConverter.ToUInt16(theStuff.ReadBytes(2), 0);
                    Console.WriteLine(blockSize);


                    theStuff.BaseStream.Seek(6 + blockStart, SeekOrigin.Begin);
                    swapper = new byte[2];

                    ccodeStuff = new byte[2];
                    long totalBlockSize = (long)(blockSize + blockStart);
                    Console.WriteLine("Total block size range is " + totalBlockSize + " and position is " + theStuff.BaseStream.Position);
                    //Console.ReadKey();
                    while (theStuff.BaseStream.Position < totalBlockSize)//Main loop of sifting through the AT7P block!
                    {

                        flagByte = (byte)theStuff.ReadByte();//Flag byte
                        Console.WriteLine("Flag byte is " + "{0:x}", flagByte);
                        flagBits = ByteToBitsString(flagByte);
                        int thePos = (int)theStuff.BaseStream.Position;
                        if (flagByte == 0xFF && totalBlockSize - theStuff.BaseStream.Position >= 8)
                        {
                            Console.WriteLine("We have an FF, let's move along");
                            rawData.AddRange(theStuff.ReadBytes(8));
                        }
                        else
                        {
                            foreach (char theBit in flagBits) //Counts from MSB to LSB
                            {
                                Console.WriteLine(theBit);

                                if (theBit.Equals('1') && theStuff.BaseStream.Position < totalBlockSize)
                                {
                                    rawData.Add((byte)theStuff.ReadByte());
                                    Console.WriteLine("It's one");
                                }
                                else if (theBit.Equals('0') && theStuff.BaseStream.Position < totalBlockSize)
                                {
                                    Console.WriteLine("It's not one");
                                    swapper[1] = (byte)theStuff.ReadByte();
                                    swapper[0] = (byte)theStuff.ReadByte();
                                    controlNybble = (byte)((swapper[1] >> 4) & 0x0F);
                                    Console.WriteLine("The control nybble is " + "{0:x}", controlNybble);
                                    ushort ccode = BitConverter.ToUInt16(swapper, 0);
                                    Console.WriteLine("The control code's whole value is " + "{0:x}", ccode);
                                    ccodeStuff[1] = (byte)(swapper[1] & 0xF);
                                    ccodeStuff[0] = swapper[0];
                                    Console.WriteLine(theStuff.BaseStream.Position);

                                    backtrack = 0x1000 - BitConverter.ToInt16(ccodeStuff, 0);
                                    //if (controlNybble == 0)
                                    //{
                                    Console.WriteLine("This control will backtrack by a value of " + "{0:x}", backtrack);
                                    for (int n = 0; n < 3 + controlNybble; n++)
                                    {
                                        try
                                        {
                                            toAdd.Add(rawData.ElementAt((rawData.Count() - backtrack) + n));
                                        }
                                        catch (System.ArgumentOutOfRangeException)
                                        {
                                            Console.WriteLine("out of bounds");
                                        }
                                    }
                                    for (int n = 0; n < 3 + controlNybble; n++)
                                    {
                                        try
                                        {
                                            rawData.Add(toAdd.ElementAt(n));
                                        }
                                        catch (System.ArgumentOutOfRangeException)
                                        {
                                            Console.WriteLine("OUT OF BOUNDS");
                                        }
                                    }
                                    toAdd.Clear();
                                    //}

                                    /*thePos = (int)theStuff.Position;


                                    theStuff.Close();
                                    theStuff = new FileStream(args[0], FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                    theStuff.Seek(thePos, SeekOrigin.Begin);*/
                                }
                            }
                        }
                        /*theDecomp.Write(rawData.ToArray());
                        if(rawData.Count() > 0x1000)
                        {
                            rawData.RemoveRange(0, rawData.Count() - 0x1000);
                        }*/
                    }
                }

                else if (header.Equals("AT7X"))
                {
                    Console.WriteLine("AT7X found, we'll just scoop these up.");
                    theStuff.BaseStream.Seek(6 + blockStart, SeekOrigin.Begin);
                    /*rawData.Add(0x41);
                    rawData.Add(0x54);
                    rawData.Add(0x37);
                    rawData.Add(0x58);
                    rawData.Add(0x00);
                    rawData.Add(0xC0);*/
                    rawData.AddRange(theStuff.ReadBytes(0xC000));
                    /*theDecomp.Write(rawData.ToArray());
                    if (rawData.Count() > 0x1000)
                    {
                        rawData.RemoveRange(0, rawData.Count() - 0x1000);
                    }*/
                }
                else if (header.Equals("AT7E")) /*We don't need the filler space in the
                decompressed file, so just stop going through the archive when we reach this part*/
                {
                    Console.WriteLine("End of the file");
                    break;
                }
                /*else
                {
                    break;
                }*/

                /*using )
                {

                    theDecomp.Write(rawData.ToArray());

                }*/



            }
            //Now we're done decompressing, so we write to the file and close it out
            theDecomp.Write(rawData.ToArray());
            theDecomp.Close();
            /*using(BinaryWriter theNewFile = new BinaryWriter(File.Open($"{theFilename}.raw", FileMode.Create)))
            {

                theNewFile.Write(rawData.ToArray());

            }*/

        }

        public static void recompressAT7P(BinaryReader theStuff, string theFilename)
        {
            toRepack = new List<byte>();
            List<byte> currentBlock = new List<byte>();
            List<byte> currentString = new List<byte>();
            List<byte> toOutput = new List<byte>();
            int maxSearch;
            
            int distAway;
            
            int totalLength = (int)theStuff.BaseStream.Length;
            //int strRange = 
            //bool blockDone;
            int[] returnedDupe = new int[2];
            //byte[] compAddress = new byte[4];
            toRepack.AddRange(theStuff.ReadBytes(totalLength));
            count = 0;

            while(count < totalLength)
            {

                if((int)totalLength - count < 0xC000)
                {
                    maxSearch = totalLength - count;
                }
                else
                {
                    maxSearch = 0xC000;
                }

                currentBlock.Clear();
                rawPos = 0;
                //blockDone = false;
                while (rawPos < maxSearch)
                {
                    flagBits = "";
                    for (int strCount = 0; strCount < 8; strCount++)
                    {
                        
                        //Console.WriteLine("strCount is " + strCount);
                        
                        returnedDupe = FindDupe(toRepack.GetRange(count, NoOOBCount(18, totalLength)), toRepack);
                        //Console.WriteLine("returnedDupe is " + returnedDupe[0]);
                        if (returnedDupe[0] != -1 && rawPos < maxSearch)
                        {
                            //Console.WriteLine("Looks like we've got a dupe to take care of, let's do it");
                            distAway = 0x1000 - (count - returnedDupe[0]);
                            controlNybble = (byte)(returnedDupe[1] - 3);
                            string codeToInsert = "";
                            //compAddress = BitConverter.GetBytes(distAway);
                            //compAddress[0] = controlNybble;
                            codeToInsert += Convert.ToString(distAway, 2);
                            codeToInsert = AddZeroes(codeToInsert, 12);
                            /*while(codeToInsert.Length < 12)
                            {
                                //Console.WriteLine("codeToInsert is now length " + codeToInsert.Length);
                                codeToInsert = codeToInsert.Insert(0, "0");
                            }*/
                            //Console.WriteLine(codeToInsert);
                            //Console.WriteLine(controlNybble);
                            //codeToInsert = codeToInsert.Remove(0, 4);
                            codeToInsert = codeToInsert.Insert(0, AddZeroes(Convert.ToString(controlNybble, 2), 4));
                            //Console.WriteLine(codeToInsert);
                            currentString.Add(Convert.ToByte(codeToInsert.Substring(0, 8), 2));
                            currentString.Add(Convert.ToByte(codeToInsert.Substring(8, 8), 2));
                            flagBits += "0";
                            //Console.WriteLine("It's a zero");
                            count += returnedDupe[1];
                            rawPos += returnedDupe[1];
                            Console.WriteLine("count: " + count + ", rawPos: " + rawPos);

                            

                        }
                        else if (returnedDupe[0] == -1 && rawPos < maxSearch)
                        {
                            currentString.Add(toRepack.ElementAt(count));
                            flagBits += "1";
                            //Console.WriteLine("It's a one; the raw byte is " + toRepack.ElementAt(count));
                            count++;
                            rawPos++;
                        }
                        
                        /*if(rawPos >= 0xC000)
                        {
                            strCount = 9;
                            flagBits = AddZeroesReverse(flagBits, 8);
                        }*/
                    }
                    //Console.WriteLine("count is " + count);
                    //Console.WriteLine("flagBits is " + flagBits);
                    flagByte = Convert.ToByte(AddZeroes(flagBits, 8), 2);
                    if(rawPos >= maxSearch)
                    {
                        flagByte = Convert.ToByte(AddZeroesReverse(flagBits, 8), 2);
                    }
                    currentString.Insert(0, flagByte);
                    currentBlock.AddRange(currentString);
                    currentString.Clear();
                    //Console.WriteLine("String compressed");
                    
                    
                    
                }
                toOutput.AddRange(at7pHeader.ToList()); //Header
                toOutput.AddRange(BitConverter.GetBytes((ushort)(currentBlock.Count() + 6)).ToList()); //Block size
                toOutput.AddRange(currentBlock); //Data
                Console.WriteLine(toOutput.Count() + "bytes through");
                Console.WriteLine(count >= totalLength);
                
                
            }

            //AT7E at the end--the game needs it
            toOutput.AddRange(at7eHeader.ToList());

            using (BinaryWriter newFileYay = new BinaryWriter(File.Open(theFilename, FileMode.Create)))
            {
                newFileYay.Write(toOutput.ToArray());
            }

        }

        /*public static int getCount()
        {
            return count;
        }*/

        /*public static int getCompIndex()
        {
            return indexOnList;
        }*/


    }
}
