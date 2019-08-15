using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace AT7View
{
    public partial class AT7View : Form
    {
        public byte[] fileThings; //Used for the filenames in loading a file and the process of file replacement
        public readonly byte[] namesEnd = 
            { 0x00, 0x00, 0x00, 0x00, 0x7C,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00 }; //This is the string of bytes at the end of the filenames in an AT7 archive
        ImageList icons = new ImageList();
        public int fileCount; //Amount of files in the archive
        public string fileName; //Name of the selected file in the archive
        public int fileNameLength; //Length of the selected file's name in the archive
        public string fileExtension; //Extension of the selected file in the archive
        public uint[] fileDetails; //Details of the selected file in the archive
        public AT7FileNode fileDetailsNode; //This takes in what's selected on the treeview
        public uint fileSize; //Size of the selected file in the archive
        public uint fileOffset; //Offset of the selected file in the archive
        public string archiveName; //Name of the AT7 container opened
        public byte[] fileBeingExtracted; //The file to be extracted from the archive
        public string fileDesc; //Filetype description in the respective label on the form
        public List<byte> fileBytes; //Used in file replacement
        public int byteDiff; //Used in resizing the AT7 archive if the inserted file differs in size from the original
        public static About aboutMenu = null; //The about menu

        
        
        
        public AT7View()
        {
            InitializeComponent();
            //Console.WriteLine(BitConverter.ToString(namesEnd));
            icons.Images.Add(Image.FromFile("icons/brick.png")); //.bin
            icons.Images.Add(Image.FromFile("icons/color_swatch.png")); //.breff
            icons.Images.Add(Image.FromFile("icons/picture_empty.png")); //.breft
            icons.Images.Add(Image.FromFile("icons/folder_palette.png")); //.brres
            icons.Images.Add(Image.FromFile("icons/cog.png")); //.dat
            icons.Images.Add(Image.FromFile("icons/script.png")); //.fsb
            icons.Images.Add(Image.FromFile("icons/page_white_gear.png")); //.ini
            icons.Images.Add(Image.FromFile("icons/picture.png")); //.jpg
            icons.Images.Add(Image.FromFile("icons/page_white_zip.png")); //.lvp
            icons.Images.Add(Image.FromFile("icons/database_table.png")); //.md
            icons.Images.Add(Image.FromFile("icons/database.png")); //.pb
            icons.Images.Add(Image.FromFile("icons/sound.png")); //.sed
            icons.Images.Add(Image.FromFile("icons/music.png")); //.smd
            icons.Images.Add(Image.FromFile("icons/application_xp.png")); //.srl
            icons.Images.Add(Image.FromFile("icons/folder_bell.png")); //.swd
            icons.Images.Add(Image.FromFile("icons/table.png")); //.tbl
            icons.Images.Add(Image.FromFile("icons/map.png")); //.tex
            icons.Images.Add(Image.FromFile("icons/page_white_code.png")); //.tlk
            icons.Images.Add(Image.FromFile("icons/page_white_text.png")); //.txt
            icons.Images.Add(Image.FromFile("icons/text_dropcaps.png")); //.utf16
            icons.Images.Add(Image.FromFile("icons/cancel.png")); //???
            treeView1.ImageList = icons;
        }

        public static UInt32 ReverseBytes(UInt32 value) //Shoutouts to the interwebz for this kind of thing
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            fileDetailsNode = e.Node.Tag as AT7FileNode;
            //Console.WriteLine(fileDetailsNode.ToString());
            infoTools.Text = fileDetailsNode.ToString();
            fileExtension = fileDetailsNode.getExt();
            fileName = fileDetailsNode.getName();
            fileOffset = fileDetailsNode.getOffset();
            fileSize = fileDetailsNode.getSize();
            fileExtInfoBox.Text = fileExtension;
            AT7FileTypes.fileTypesDict.TryGetValue(fileExtension, out fileDesc); //Nabs the file description for the display label
            fileTypeDesc.Text = fileDesc;
            buttonExtract.Enabled = true;
            buttonExportAs.Enabled = false;
            buttonExportAs.Text = "Export As";
            if (fileName.Equals("gmm.txt"))
            {
                buttonExportAs.Enabled = true;
                buttonExportAs.Text = "Export .csv";
            }
            buttonReplace.Enabled = true;
            //Console.WriteLine(treeView1.Nodes[0].Tag.ToString());
            //Console.WriteLine("Offset: {0:X}", fileDetailsNode.getOffset() + "\n Size: {0:X}", fileDetailsNode.getSize());
        }

        private void loadThatFile()
        {
            
            fileCount = 0;
            treeView1.Nodes.Clear();
            BinaryReader quickie = new BinaryReader(File.Open(archiveName, FileMode.Open));
            fileThings = new byte[0x1C];

            fileThings = quickie.ReadBytes(0x1C);
            if (fileThings[0] == 0x41 && fileThings[1] == 0x54 && fileThings[2] == 0x37 && fileThings[3] == 0x50)
            {
                /*DialogResult pleaseWait = new DialogResult();
                pleaseWait = MessageBox.Show("Decompressing file...");*/
                //fileTypeDesc.Text = $"Compressed data opened. Decompressing to {Path.GetFileName(archiveName)}";
                AT7FileProcessor.decompressAT7P(quickie, archiveName);
                archiveName = archiveName.Insert(archiveName.Length, ".raw");
                DialogResult decompressedTheFile = new DialogResult();
                decompressedTheFile = MessageBox.Show($"Archive decompressed as {Path.GetFileName(archiveName)}. Be sure to open this one from now on and not the one you just opened.");
                //fileTypeDesc.ResetText();

            }
            quickie.Close();
            Console.WriteLine(archiveName);
            using (BinaryReader fileReader = new BinaryReader(File.Open(archiveName, FileMode.Open)))
            {
                
                

                Console.WriteLine(archiveName);
                //progressBarExtract.Maximum = (int)fileReader.BaseStream.Length;
                
                fileThings = new byte[0x1C];

                fileThings = fileReader.ReadBytes(0x1C);

                

                    //fileDetailsNode = new List<uint>();
                    while (!Enumerable.SequenceEqual(fileThings, namesEnd))
                {
                    Console.WriteLine(BitConverter.ToString(fileThings));

                    //Now to prepare the filename's real length to remove the junk text
                    fileNameLength = 0;
                    for (int i = 8; fileThings[i] != 0x00; i++)
                    {
                        fileNameLength++;
                    }
                    fileName = Encoding.UTF8.GetString(fileThings, 8, fileNameLength);
                    TreeNode toAdd = new TreeNode(fileName);

                    //Now let's nab that file extension
                    fileExtension = "???";
                    foreach (string thing in AT7FileTypes.fileTypes)
                    {
                        if (fileName.Contains(thing))
                        {
                            fileExtension = thing;
                        }
                    }


                    AT7FileNode currentNode = new AT7FileNode(ReverseBytes(BitConverter.ToUInt32(fileThings, 4)), ReverseBytes(BitConverter.ToUInt32(fileThings, 0)), fileName, fileExtension);
                    /*fileOffset = ReverseBytes(BitConverter.ToUInt32(fileThings, 0));
                    fileSize = ReverseBytes(BitConverter.ToUInt32(fileThings, 4));
                    fileDetailsNode.Add(fileOffset); fileDetailsNode.Add(fileSize);*/


                    toAdd.Tag = currentNode;
                    toAdd.ImageIndex = AT7FileTypes.fileTypes.IndexOf(fileExtension);
                    toAdd.SelectedImageIndex = AT7FileTypes.fileTypes.IndexOf(fileExtension);
                    treeView1.Nodes.Add(toAdd);
                    /*fileName = Encoding.UTF8.GetString(fileThings, 8, fileNameLength); //Aaaaaaaand filename applied
                    //fileName = fileName.Substring(0, fileName.IndexOf("?"));
                    //TreeNode fileNode new TreeNode();*/


                    Console.WriteLine(treeView1.Nodes[fileCount].Text);
                    fileCount++;
                    fileThings = fileReader.ReadBytes(0x1C); //While loop ends with this

                }

            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Choose a dataX_XXXX file to open.";
            openFile.Filter = "AT7 archives|*.bin;*.bin.raw|All files (*.*)|*.*";
            openFile.FilterIndex = 0;
            DialogResult result = openFile.ShowDialog();
            //Console.WriteLine(this.backgroundWorker1.IsBusy);
            archiveName = openFile.FileName;

            if (result == DialogResult.OK)
            {
                //fileTypeDesc.Text = "Opening file, please give it some time.";
                Cursor.Current = Cursors.WaitCursor;
                /*Cursor.Current = Cursors.WaitCursor;
                this.backgroundWorker1.RunWorkerAsync();
                while (this.backgroundWorker1.IsBusy)
                {
                    Console.WriteLine(AT7FileProcessor.getCount());
                    progressBarExtract.Value = AT7FileProcessor.getCount();
                    //Application.DoEvents();
                }
                Cursor = Cursors.Default;*/
                loadThatFile();
                Cursor = Cursors.Default;
                recompressToolStripMenuItem.Enabled = true;
                buttonExtractAll.Enabled = true;
                //fileTypeDesc.ResetText();

            }
        }
        
        /*private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //AllocConsole();
            loadThatFile();
            //FreeConsole();
        }*/


            private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            aboutMenu = new About();
            aboutMenu.Show();
            
                
            
        }
        
        private void AT7View_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //EXTRACT FILE
        private void buttonExtract_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog toExtract = new SaveFileDialog();
            
            toExtract.Filter = $"{fileExtension} files|*{fileExtension}|All files (*.*)|*.*";
            toExtract.FilterIndex = 0;
            toExtract.OverwritePrompt = true;
            toExtract.RestoreDirectory = true;
            toExtract.FileName = fileName;
            toExtract.Title = $"Extract {fileName} where?";

            if (toExtract.ShowDialog() == DialogResult.OK)
            {
                
                fileBeingExtracted = new byte[fileSize];
                
                using (BinaryReader fileReader = new BinaryReader(File.Open(archiveName, FileMode.Open))) //Read the open archive to grab what we want to extract
                {
                    fileReader.BaseStream.Position = fileOffset;
                    fileReader.ReadBytes((int)fileSize).CopyTo(fileBeingExtracted, 0);
                    
                }
                
                using (BinaryWriter outputFile = new BinaryWriter(File.Open(toExtract.FileName, FileMode.Create))) //Output what was ripped from the archive to a new file on disk
                {
                    Console.WriteLine(fileName);
                    for (int k = 0; k < fileSize; k++)
                    {
                        outputFile.Write(fileBeingExtracted[k]);
                    }

                    
                }

                DialogResult extractedYay = new DialogResult();
                extractedYay = MessageBox.Show($"{Path.GetFileName(toExtract.FileName)} has been extracted");


            }
        }
        //EXTRACT ALL FILES
        private void buttonExtractAll_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog toExtractTo = new FolderBrowserDialog();
            toExtractTo.Description = "Extract the contents to where?";
            
            DialogResult result = toExtractTo.ShowDialog();

            if(result == DialogResult.OK)
            {
                BinaryWriter currentOutput;
                string selFolder = toExtractTo.SelectedPath;
                using (BinaryReader fileReader = new BinaryReader(File.Open(archiveName, FileMode.Open))) //Read the open archive to go through all the files
                {
                    foreach (TreeNode currentFile in treeView1.Nodes)
                    {
                        AT7FileNode dealingWith = currentFile.Tag as AT7FileNode;
                        Console.WriteLine($"{selFolder}\\{dealingWith.getName()}");
                        currentOutput = new BinaryWriter(File.Open($"{selFolder}\\{dealingWith.getName()}", FileMode.Create));
                        fileReader.BaseStream.Position = dealingWith.getOffset();
                        currentOutput.Write(fileReader.ReadBytes((int)dealingWith.getSize()));
                        currentOutput.Close();

                    }
                    

                }
                
                DialogResult extractedYay = new DialogResult();
                extractedYay = MessageBox.Show($"All files of {Path.GetFileName(archiveName)} extracted to {selFolder}.");
            }
        }
        //EXPORT FILE AS
        private void buttonExportAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog toExtract = new SaveFileDialog();
            string newExt = ""; //This is the file extension of the exported file

            if(buttonExportAs.Text.Equals("Export .csv"))
            {
                newExt = ".csv";
            }

            toExtract.Filter = $"{newExt} files|*{newExt}|All files (*.*)|*.*";
            toExtract.FilterIndex = 0;
            toExtract.OverwritePrompt = true;
            toExtract.RestoreDirectory = true;
            toExtract.FileName = fileName.Replace(fileExtension, newExt);
            toExtract.Title = $"Export {toExtract.FileName} where?";

            if (toExtract.ShowDialog() == DialogResult.OK)
            {
                if (newExt.Equals(".csv"))
                {
                    using (StreamWriter csvExport = new StreamWriter(toExtract.FileName, false, Encoding.GetEncoding("shift-jis")))
                    {

                        fileBeingExtracted = new byte[fileSize];

                        using (BinaryReader fileReader = new BinaryReader(File.Open(archiveName, FileMode.Open))) //Read the open archive to grab what we want to extract
                        {
                            fileReader.BaseStream.Position = fileOffset;
                            fileReader.ReadBytes((int)fileSize).CopyTo(fileBeingExtracted, 0);

                        }

                        using (BinaryWriter outputFile = new BinaryWriter(File.Open("temp.txt", FileMode.Create))) //Output what was ripped from the archive to a new file on disk
                        {
                            Console.WriteLine(fileName);
                            for (int k = 0; k < fileSize; k++)
                            {
                                outputFile.Write(fileBeingExtracted[k]);
                            }


                        }
                        StreamReader toTakeFrom = new StreamReader("temp.txt", Encoding.GetEncoding("shift-jis"));

                        string nextLine = "";
                        //byte[] nextString;
                        //bool exceptionThrown = false;
                        while (!nextLine.Equals("$$$")) //gmm.text ends with "$$$"
                        {
                            try
                            {
                                nextLine = toTakeFrom.ReadLine();
                                //nextLine = Convert.ToBase64String(nextString); //Encoding.GetEncoding("shift-jis"));
                                if (nextLine.IndexOf("001=") == 0) //This is what the very beginning of gmm starts with
                                {
                                    nextLine = nextLine.Remove(0, 4);
                                }
                                else if (nextLine.IndexOf("=") == 0) //Everything else that isn't the end starts with this
                                {
                                    nextLine = nextLine.Remove(0, 1);
                                }
                                csvExport.WriteLine($"\"{nextLine}\"");
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                                break;
                            }
                        }
                        toTakeFrom.Close();
                    }
                }
                DialogResult extractedYay = new DialogResult();
                extractedYay = MessageBox.Show($"{Path.GetFileName(toExtract.FileName)} has been exported.");
            }

        }
        //REPLACE FILE
        private void buttonReplace_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog toReplace = new OpenFileDialog();
            toReplace.Title = $"Replace {fileName} with what?";
            DialogResult result = toReplace.ShowDialog();
            //DialogResult.
            if (result == DialogResult.OK)
            {
                fileBytes = new List<byte>(); //fileBytes is to be a List containing all the bytes of the archive that had a file replacement
                BinaryReader fileBeingInserted = new BinaryReader(File.Open(toReplace.FileName, FileMode.Open));
                using (BinaryReader fileReader = new BinaryReader(File.Open(archiveName, FileMode.Open)))
                {
                    fileBytes.AddRange(fileReader.ReadBytes((int)fileReader.BaseStream.Length));
                    fileThings = new byte[fileBeingInserted.BaseStream.Length];
                    /*for(int i = 0; i < fileReader.BaseStream.Length; i++)
                    {
                        fileBytes.Add(fileReader.ReadByte());
                    }
                    fileReader.*/
                    fileBytes.RemoveRange((int)fileOffset, (int)fileSize);
                    /*for(int k = 0; k < fileBeingInserted.BaseStream.Length; k++)
                    {
                        fileBytes.Insert((int)fileOffset + k, fileBeingInserted.ReadByte());
                    }*/
                    fileBeingInserted.ReadBytes((int)fileBeingInserted.BaseStream.Length).ToList().CopyTo(fileThings);
                    fileBytes.InsertRange((int)fileOffset, fileThings);

                    //Now to alter the file offsets accordingly
                    byteDiff = (int)(fileBeingInserted.BaseStream.Length - fileSize);
                    fileThings = new byte[0x1C];
                    byte[] fourBytes = new byte[4];
                    //fourBytes = BitConverter.GetBytes(fileOffset - 4);
                    

                    int namesCount = 0;
                    while (!Enumerable.SequenceEqual(fileThings, namesEnd))
                    {
                        fileBytes.GetRange(namesCount, 0x1C).CopyTo(fileThings);
                        uint tehOffset = ReverseBytes(BitConverter.ToUInt32(fileThings, 0));
                        //Console.WriteLine(tehOffset);
                        if (tehOffset > fileOffset) //All files after the inserted one need to have their offsets shifted
                        {
                            tehOffset += (uint)byteDiff;
                            //Console.WriteLine(tehOffset);
                            fourBytes = BitConverter.GetBytes(tehOffset);
                            //Console.WriteLine(fourBytes[0] + " " + fourBytes[1] + " " + fourBytes[2] + " " + fourBytes[3]);
                            fileBytes[namesCount] = fourBytes[3];
                            fileBytes[namesCount + 1] = fourBytes[2];
                            fileBytes[namesCount + 2] = fourBytes[1];
                            fileBytes[namesCount + 3] = fourBytes[0];
                        }
                        else if(tehOffset == fileOffset) //The replaced file in the archive needs its size to represent the new file
                        {
                            Console.WriteLine("Found it");
                            fourBytes = BitConverter.GetBytes((uint)fileBeingInserted.BaseStream.Length);
                            fileBytes[7 + namesCount] = fourBytes[0];
                            fileBytes[6 + namesCount] = fourBytes[1];
                            fileBytes[5 + namesCount] = fourBytes[2];
                            fileBytes[4 + namesCount] = fourBytes[3];
                        }
                        
                        namesCount += 0x1C;
                    }


                    /*using (BinaryWriter debugWrite = new BinaryWriter(File.Open("test", FileMode.Create)))
                    {
                        //fileBeingInserted.BaseStream.Position = 0;
                        //for (int i = 0; i < fileBytes.Count(); i++)
                        //{
                            //debugWrite.Write(fileBytes.ElementAt(i));
                        //}
                        debugWrite.Write(fileBytes.ToArray(), 0, fileBytes.Count());
                    }*/

                    
                    

                }
                fileBeingInserted.Close();
                using (BinaryWriter ogFile = new BinaryWriter(File.Open(archiveName, FileMode.Create)))
                {

                    ogFile.Write(fileBytes.ToArray(), 0, fileBytes.Count());
                }
                treeView1.Nodes.Clear();
                buttonExtract.Enabled = false;
                
                buttonReplace.Enabled = false;
                fileExtInfoBox.Text = "";
                DialogResult replacedYay = new DialogResult();
                replacedYay = MessageBox.Show($"{fileName} has been replaced.");
                //fileTypeDesc.Text = $"{fileName} has been replaced.";
                loadThatFile();
                
            }
        }

        private void fileExtInfoBox_Enter(object sender, EventArgs e)
        {

        }
        
        private void progressBarExtract_Click(object sender, EventArgs e)
        {

        }

        private void recompressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog toOutput = new SaveFileDialog();
            toOutput.Title = $"Save recompressed archive to where?";
            toOutput.Filter = $"bin files|*.bin|All files (*.*)|*.*";
            toOutput.FilterIndex = 0;
            toOutput.OverwritePrompt = true;
            toOutput.RestoreDirectory = true;
            toOutput.FileName = Path.GetFileName(archiveName.Replace(".raw", "")); //This won't be a raw file so we take that extension out
            DialogResult result = toOutput.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                DateTime recompStart = DateTime.Now;
                AT7FileProcessor.recompressAT7P(new BinaryReader(File.Open(archiveName, FileMode.Open)), toOutput.FileName);
                Cursor = Cursors.Default;
                DateTime recompEnd = DateTime.Now;
                using (TextWriter datesLog = File.CreateText("dates.log")) //Just in case that end popup gets clicked through a little too fast
                {
                    datesLog.WriteLine("Start: " + recompStart.ToString());
                    datesLog.WriteLine("Finish " + recompEnd.ToString());
                }
                DialogResult recompressedTheFile = new DialogResult();
                recompressedTheFile = MessageBox.Show("Recompressed the file.\n" + "Start: " + recompStart.ToString() + "\nFinish: " + recompEnd.ToString());
            }
        }

        
    }
}
