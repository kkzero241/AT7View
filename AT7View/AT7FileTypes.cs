using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT7View
{
    public class AT7FileTypes
    {
        public static List<string> fileTypes = new List<string>
        {
            {".bin"},
            {".breff"},
            {".breft"},
            {".brres"},
            {".dat"},
            {".fsb"},
            {".ini"},
            {".jpg"},
            {".lvp"},
            {".md"},
            {".pb"},
            {".sed"},
            {".smd"},
            {".srl"},
            {".swd" },
            {".tbl"},
            {".tex"},
            {".tlk"},
            {".txt"},
            {".utf16"},
            {"???"}
            
            
        };

        public static IDictionary<string, string> fileTypesDict = new Dictionary<string, string>
        {
            {".bin", ".bin is an ambiguous extension given to binary data. "},
            {".breff", ".breff is a collection of particle data. The format is used an various first-party Wii titles."},
            {".breft", ".breft is an image format that can be found in various first-party Wii titles."},
            {".brres", ".brres is a collection of graphical data such as models, textures, palettes, ands animations. Many first-party Wii games use this format."},
            {".dat", "Like .bin, .dat is a vague extension. In PMD WiiWare, it is used to blueprint mechanics such as attacks."},
            {".fsb", ".fsb seems to be a scripting collection for overworld events."},
            {".ini", ".ini is a longtime standard for configuration files. In PMD WiiWare, it is seemingly only used for debug-related content."},
            {".jpg", ".jpg is the infamous standard in lossily-compressed image files."},
            {".lvp", ".lvp seems to be related to storing Pokémon data. It has multiple AT7P blocks."},
            {".md", ".md seems to be another format for storing Pokémon data. Not to be confused with plaintext markdown."},
            {".pb", ".pb seems to be used for storing gameplay-related data, as implied by the one file that uses the extension."},
            {".srl", ".srl is the format for DS Download Play apps, like PMD WiiWare's option to use a DS as a controller."},
            {".smd", ".smd is a sequenced music format very much like MIDI."},
            {".tex", ".tex is an SIR0 container which stores uncompressed image data."},
            {".tbl", ".tbl is a binary text tabling format. Not the same .tbl format born out of the romhacking community."},
            {".tlk", ".tlk seems to be related to text/dialogue."},
            {".txt", ".txt is the number one standard in plain text file extensions. In PMD WiiWare, it is used not only for the game's Shift-JIS text, but also some dungeon-related data."},
            {".utf16", ".utf16 is an extension named for the text encoding standard. In PMD WiiWare, it is only used for the save banner data."},
            {".sed", ".sed is a format for sequenced sound effects."},
            {".swd", ".swd is the collection of presets and samples utilized by .smd music files."},
            {"???", "This file extension does not exist in the PMD WiiWare games." }
        };
    }
}
