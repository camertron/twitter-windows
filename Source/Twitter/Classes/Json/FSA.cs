using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Twitter.Json
{
    /// <summary>
    /// This class represents a finite state automaton (FSA).  It is fed characters
    /// from a source file, one at a time.  These characters cause the FSA to change
    /// state.  When an accepting state is found, the characters that brought the FSA
    /// from the starting state to that accepting state are turned into tokens by the
    /// Tokenizer class.
    /// </summary>
    public class FSA
    {
        /// <summary>
        /// The current state of this machine.
        /// </summary>
        private FSAState m_stCurrentState;

        /// <summary>
        /// All possible states.
        /// </summary>
        private List<FSAState> m_lfsAllStates;

        /// <summary>
        /// The last state this machine was in.
        /// </summary>
        private FSAState m_stLastState;

        /// <summary>
        /// Whether or not the machine will back up after accepting a string.
        /// </summary>
        private bool bBackUp;

        /// <summary>
        /// The source code language being fed into this FSA.
        /// </summary>
        private string sLanguage;

        //ASCII digit and character ranges
        private const int C_DIGIT_START = 48;
        private const int C_DIGIT_END = 57;
        private const int C_LETTER_LCASE_START = 97;
        private const int C_LETTER_UCASE_START = 65;
        private const int C_LETTER_LCASE_END = 122;
        private const int C_LETTER_UCASE_END = 90;

        //space character ASCII code
        private const int C_SPACE_CODE = 32;

        //special supported character literals.  $d stands for any digit, etc
        private const string C_LETTER_INDICATOR = "$l";
        private const string C_DIGIT_INDICATOR = "$d";
        private const string C_PUNCTUATION_INDICATOR = "$p";
        private const string C_SPACE_INDICATOR = "$sp";
        private const string C_LINEFEED_INDICATOR = "$n";
        private const string C_CARRIAGE_RETURN_INDICATOR = "$r";
        private const string C_SGL_QUOTE_INDICATOR = "$sq";
        private const string C_DBL_QUOTE_INDICATOR = "$dq";
        private const string C_TAB_INDICATOR = "$t";
        private const string C_EOF_INDICATOR = "$EOF";
        private const string C_COMMA_INDICATOR = "$comma";

        /// <summary>
        /// Constructor.  Makes a new FSA and initializes the current state to null.
        /// </summary>
        public FSA()
        {
            m_stCurrentState = null;
            m_lfsAllStates = new List<FSAState>();
            m_stLastState = null;
        }

        /// <summary>
        /// Resets this FSA to its starting state.
        /// </summary>
        public void Reset()
        {
            m_stCurrentState = m_lfsAllStates[0];
            bBackUp = false;
        }

        public void FeedEOF()
        {
            m_stLastState = m_stCurrentState;
            m_stCurrentState = m_lfsAllStates[((int)m_stCurrentState.InputStates[C_EOF_INDICATOR])];
            bBackUp = m_stCurrentState.BackUp;
        }

        /// <summary>
        /// Inputs a character into this FSA and causes it to change state.
        /// </summary>
        /// <param name="cFood">The input character.</param>
        public void Feed(char cFood)
        {
            string sInput;

            //handle special cases like $d = all digits
            if (IsDigit(cFood))
                sInput = C_DIGIT_INDICATOR;
            else if (IsLetter(cFood))
                sInput = C_LETTER_INDICATOR;
            else if (IsSpace(cFood))
                sInput = C_SPACE_INDICATOR;
            else if (cFood == '\r')
                sInput = C_CARRIAGE_RETURN_INDICATOR;
            else if (cFood == '\n')
                sInput = C_LINEFEED_INDICATOR;
            else if (cFood == '\"')
                sInput = C_DBL_QUOTE_INDICATOR;
            else if (cFood == '\'')
                sInput = C_SGL_QUOTE_INDICATOR;
            else if (cFood == ',')
                sInput = C_COMMA_INDICATOR;
            else if (cFood == '\t')
                sInput = C_TAB_INDICATOR;
            else
                sInput = cFood.ToString();  //otherwise, just use character

            //if the input character cannot be found in the current state's state table,
            //treat it as general punctuation
            if (! m_stCurrentState.InputStates.ContainsKey(sInput))
                sInput = C_PUNCTUATION_INDICATOR;

            //remember last state, get current state, determine if a back up is needed
            m_stLastState = m_stCurrentState;
            m_stCurrentState = m_lfsAllStates[((int)m_stCurrentState.InputStates[sInput])];
            bBackUp = m_stCurrentState.BackUp;
        }

        /// <summary>
        /// Gets true if the FSA has reported the need for a backup, false otherwise.
        /// </summary>
        public bool BackUp
        {
            get { return bBackUp; }
        }

        /// <summary>
        /// Determines if the given character is a numerical digit from 0-9.
        /// </summary>
        /// <param name="cDigit">The character to examine.</param>
        /// <returns>Returns true if cDigit is one of 0-9, false otherwise.</returns>
        private bool IsDigit(char cDigit)
        {
            //get ASCII character code for range checking purposes
            int iCharCode = (int)cDigit;

            //luckily, ASCII digits are mapped to character codes linearly, so
            //by checking if the charcode falls within a certain range, we can determine
            //if the given character is a digit or not
            bool bFinal = (iCharCode >= C_DIGIT_START);
            bFinal &= (iCharCode <= C_DIGIT_END);

            return bFinal;
        }

        /// <summary>
        /// Determines if the given character is an alphabetic character from a-z or A-Z.
        /// </summary>
        /// <param name="cLetter">The character to examine.</param>
        /// <returns>Returns true if cDigit is one of a-z or A-Z, false otherwise.</returns>
        private bool IsLetter(char cLetter)
        {
            //get ASCII character code for range checking purposes
            int iCharCode = (int)cLetter;

            //luckily, ASCII letters are mapped to character codes linearly, so
            //by checking if the charcode falls within a certain range, we can determine
            //if the given character is uppercase, lowercase, or neither

            bool bFirstFinal = (iCharCode >= C_LETTER_LCASE_START);
            bFirstFinal &= (iCharCode <= C_LETTER_LCASE_END);

            bool bSecondFinal = (iCharCode >= C_LETTER_UCASE_START);
            bSecondFinal &= (iCharCode <= C_LETTER_UCASE_END);

            return (bFirstFinal | bSecondFinal);
        }

        /// <summary>
        /// Determines if the given character is a space.
        /// </summary>
        /// <param name="cSpace">The character to examine.</param>
        /// <returns>Returns true if the character is a space, false otherwise.</returns>
        private bool IsSpace(char cSpace)
        {
            int iCharCode = (int)cSpace;
            return (iCharCode == C_SPACE_CODE);
        }

        /// <summary>
        /// Gets true if the current state is an accepting state, false otherwise.
        /// </summary>
        public bool Accepted
        {
            get { return (m_stCurrentState.Accepting == true); }
        }

        /// <summary>
        /// Gets the last state this machine was in.
        /// </summary>
        public FSAState LastState
        {
            get { return m_stLastState; }
        }

        /// <summary>
        /// Gets the source code language.
        /// </summary>
        public string Language
        {
            get { return sLanguage; }
        }

        /// <summary>
        /// Gets the current state of this machine.
        /// </summary>
        public FSAState CurrentState
        {
            get { return m_stCurrentState; }
        }

        /// <summary>
        /// Loads a CSV state table from a stream.
        /// </summary>
        /// <param name="srInputReader">The stream to read the state table from.</param>
        /// <returns>Returns an FSA containing states built from the given stream.</returns>
        public static FSA FromStream(StreamReader srInputReader)
        {
            FSA fsaFinal = new FSA();
            string[] saStates = srInputReader.ReadLine().Split(FSAState.c_acSplitters);

            while (!srInputReader.EndOfStream)
                fsaFinal.m_lfsAllStates.Add(FSAState.FromString(srInputReader.ReadLine(), saStates));

            srInputReader.Close();

            return fsaFinal;
        }

        /// <summary>
        /// Loads a CSV state table from a file.
        /// </summary>
        /// <param name="sFileName">The file to load the state table from.</param>
        /// <returns>Returns an FSA containing states built from the given file.</returns>
        public static FSA FromFile(string sFileName)
        {
            if (File.Exists(sFileName))
            {
                //get stream by opening file, pass into FromStream
                return FromStream(new StreamReader(sFileName));
            }
            else
                return null;  //file doesn't exist, return null
        }
    }

    /// <summary>
    /// Encapsulates data about a finite state used in FSAs.
    /// </summary>
    public class FSAState
    {
        private bool m_bAccepting;                                  //this state is an accepting state or not
        private Hashtable m_htInputStates;                          //inputs and corresponding states
        private bool m_bBackUp;                                     //whether or not this state requires us to back up in the file stream
        private string m_sDescription;                              //this state's description, (ex:  Found <>)
        private int iStateNumber;                                   //this state's index
        public static char[] c_acSplitters = new char[1] { ',' };   //CSV delimiters used when reading in an FSA from a file or stream

        /// <summary>
        /// Default constructor.  Initializes a new instance of FSAState that is not accepting,
        /// has no description, and has no state transitions.
        /// </summary>
        public FSAState()
        {
            m_bAccepting = false;
            m_htInputStates = new Hashtable();  //capacity?  loadfactor?
            m_sDescription = "";
        }

        /// <summary>
        /// Gets or sets whether this state is an accepting state.
        /// </summary>
        public bool Accepting
        {
            get { return m_bAccepting; }
        }

        /// <summary>
        /// Gets the Hashtable containing state transitions.
        /// </summary>
        public Hashtable InputStates
        {
            get { return m_htInputStates; }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if the input stream needs to be backed up.
        /// </summary>
        public bool BackUp
        {
            get { return m_bBackUp; }
            set { m_bBackUp = value; }
        }

        /// <summary>
        /// Gets or sets the description of this state.
        /// </summary>
        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        /// <summary>
        /// Gets or sets the index of this state.
        /// </summary>
        public int StateNumber
        {
            get { return iStateNumber; }
            set { iStateNumber = value; }
        }

        /// <summary>
        /// Builds a state from a string and returns it.
        /// </summary>
        /// <param name="sStateStr">The string to use when constructing the FSAState.</param>
        /// <returns>Returns a new FSAState containing the information found in the specified string.</returns>
        public static FSAState FromString(string sStateStr, string[] saStates)
        {
            FSAState fsasFinal = new FSAState();
            string[] saParts = sStateStr.Split(c_acSplitters);

            fsasFinal.m_sDescription = saParts[0];
            fsasFinal.m_bAccepting = saParts[1].Contains("#");

            for (int i = 2; i < saParts.Length - 1; i++)
                fsasFinal.m_htInputStates.Add(saStates[i], Int32.Parse(saParts[i]) - 1);  //-1 because array starts at 0

            fsasFinal.m_bBackUp = (saParts[saParts.Length - 1] == "y");

            //return the completed state
            return fsasFinal;
        }
    }
}
