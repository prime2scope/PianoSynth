
/***********************************************************************
Program:     Piano Synth           
             
Date:        12/6/2017
             
Programmer:  Ben Stockwell

Purpose: This application allows you to play a "Synthesized Piano"

(we were told to create something interesting/useful with a C# form
and this is what I chose)

You can control the Piano by Clicking the Keys OR you can use your
keyboard to play the piano.

The White Keys on the Left start at ZXCVBNM then move to QWERTYUIOP
The Black Keys are controlled by the Keys inbetween each of the keys 
listed above, aka - SD, GHJ, 23, 567, 90.

You can control the Volume of the sounds, and you can Also change the 
Octave of the Keyboard between:

C1 which is 32 HZ sine Wave
C9 which is 8372 HZ sine Wave

The max duration of the sign wave can also be controlled
and you can reset everything to default with the Reset button

NOTE: Windows Media Player was required to play MORE than 1 Sound at
a time, this required the Sine Waves to be output as Audio Files.

The Files should only be created if they dont already exist.
(Creating Unique files for every combo of Volume, Pitch, and Length
is definitly a bad idea - it was just easy to implement at the time)

(There are some alternatives, however they require special drivers
or libraries - and would not work unless the OS had them)

NOTE: Mashing too many keys at the same time MAY cause the program 
to Lag, please be gentle.

I hope you enjoy!

***********************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using WMPLib;
using System.IO;


namespace PianoSynth
{
    public partial class Form1 : Form
    {
        #region Global Variables

        // Saves the position of the moving labels
        Point OC0pos;
        Point OC1pos;
        Point OC2pos;

        const UInt16 MAXVOL = 65000;        // Max Volume to Create Sound Files

        static int gPressColor = 180;       // Color Changed to when Piano Key Press
        static int gDuration = 6000;        // Duration of Piano Sounds
        static UInt16 gVolume = 58500;      // Default Volume for Sounds

        static int OctaveOffset = 72;       // Default Octave of Piano on Program Start

        double[] NoteFreq = new double[127];    // Holds ALL possible Note Frequencies that will be used

        int[] kIndex = new int[29];     // Holds the index for the specific Key to use from the NoteFreq Table

        #region Media Players
        // Windows Media Player Objects
        // Used to PLAY the sound File
        // Must use these to play MORE than 1 sound at a time
        static WMPLib.WindowsMediaPlayer gpl_0 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_1 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_2 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_3 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_4 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_5 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_6 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_7 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_8 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_9 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_10 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_11 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_12 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_13 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_14 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_15 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_16 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_17 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_18 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_19 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_20 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_21 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_22 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_23 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_24 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_25 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_26 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_27 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_28 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer gpl_29 = new WMPLib.WindowsMediaPlayer();
        #endregion

        #region Playing Flags
        // Flags to Prevent Conflicts if a Key is already being played
        static Boolean isPlaying0 = false;
        static Boolean isPlaying1 = false;
        static Boolean isPlaying2 = false;
        static Boolean isPlaying3 = false;
        static Boolean isPlaying4 = false;
        static Boolean isPlaying5 = false;
        static Boolean isPlaying6 = false;
        static Boolean isPlaying7 = false;
        static Boolean isPlaying8 = false;
        static Boolean isPlaying9 = false;
        static Boolean isPlaying10 = false;
        static Boolean isPlaying11 = false;
        static Boolean isPlaying12 = false;
        static Boolean isPlaying13 = false;
        static Boolean isPlaying14 = false;
        static Boolean isPlaying15 = false;
        static Boolean isPlaying16 = false;
        static Boolean isPlaying17 = false;
        static Boolean isPlaying18 = false;
        static Boolean isPlaying19 = false;
        static Boolean isPlaying20 = false;
        static Boolean isPlaying21 = false;
        static Boolean isPlaying22 = false;
        static Boolean isPlaying23 = false;
        static Boolean isPlaying24 = false;
        static Boolean isPlaying25 = false;
        static Boolean isPlaying26 = false;
        static Boolean isPlaying27 = false;
        static Boolean isPlaying28 = false;
        static Boolean isPlaying29 = false;
        #endregion

        #endregion

        #region Form Initialization Functions

        public Form1()
        {
            InitializeComponent();
        }

        // Form initialization stuff
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Get all the Note frequencies that will be used by the piano
                InitNoteFrequency();

                // Save the label locations
                OC0pos = OCLabel0.Location;
                OC1pos = OCLabel1.Location;
                OC2pos = OCLabel2.Location;

            }
            catch(Exception ex)
            {
                // Output the exception
                Console.WriteLine("Init Excp Thrown : " + ex);
            }

        }


        // Used to calculate all of the potential Note frequencies
        // for a common piano, goes a little beyond 88 keys
        // Range from "C-1" to "G9"
        // aka 8.176 HZ to 12543.9 HZ
        private void InitNoteFrequency()
        {
            // Loop through to get each Frequency in Hertz
            for (int i = 0; i < 127; i++)
            {
                // Calculate the exact Frequency for the given
                NoteFreq[i] = (440.0 * Math.Pow(2.0, ( ((double)i - 69.0) / 12.0) ));
                //Console.WriteLine("Adding Freq = " + NoteFreq[i]);
            }

            // Initialize the Key Index
            // This will be used with the piano Key number to get the 
            // frequency for the given key
            for (int x = 0; x < 29; x++)
            {
                kIndex[x] = x + OctaveOffset;
                //Console.WriteLine(x + "kIndex Freq = " + kIndex[x]);
            }

        }

        #endregion

        #region Option Control Event Handlers

        // Used to show the Volume as a Tool Tip with a Percentage
        private void VolumeBar1_Scroll(object sender, EventArgs e)
        {
            // Multiply by 10 to show percentage
            int tval = VolumeBar1.Value * 10;

            // Show the tool tip
            VolumeTip1.SetToolTip(VolumeBar1, tval.ToString() + "%");

            // Set the Global Volume
            gVolume = (UInt16)((VolumeBar1.Value / 10.0) * MAXVOL);
        }

        // Sets the MAX length of that the Sine wave will play for 
        private void SetNoteLength_ValueChanged(object sender, EventArgs e)
        {
            // Get the duration and set the global
            float m = (float)(SetNoteLength.Value * 1000);
            gDuration = (int)m;

            // Reset the focus
            // Must be done or error sounds play when pressing keyboard
            PianoPanel1.Focus();
        }

        // Set the Octave Offset for the Piano Keys
        // This will make the keys play a sound that is Higher or Lower
        // This also adjusts the Labels to Show which keys are at what Octave
        private void SetOctave_ValueChanged(object sender, EventArgs e)
        {
            // If Octave is MAX or MIN and Semitone is NOT 0
            if ((SetOctave.Value == SetOctave.Maximum || SetOctave.Value == SetOctave.Minimum) && SetSemitone.Value != 0)
            {
                // If Octave is MAX/MIN, we do NOT allow Semitone to Change
                // Disconnect Event Handler so semitone event isn't Triggered
                this.SetSemitone.ValueChanged -= SetSemitone_ValueChanged;
                SetSemitone.Value = 0;
                this.SetSemitone.ValueChanged += SetSemitone_ValueChanged;
            }


            // Get the Octave Offset
            OctaveOffset = ((int)SetOctave.Value + 1) * 12;
            
            // Increment it by the Semitone offset
            OctaveOffset += (int)SetSemitone.Value;

            // Re-Initialize the Key Index
            // This will be used with the piano Key number to get the 
            // frequency for the given key
            for (int x = 0; x < 29; x++)
            {
                kIndex[x] = x + OctaveOffset;
                Console.WriteLine(x + "  kIndex Freq = " + kIndex[x]);
            }

            // Set the Octave Label Text
            // Compensate for the Semitone Offset
            OCLabel0.Text = "C" + (((OctaveOffset - (int)SetSemitone.Value) / 12) - 1);
            OCLabel1.Text = "C" + ((OctaveOffset - (int)SetSemitone.Value) / 12);
            OCLabel2.Text = "C" + (((OctaveOffset - (int)SetSemitone.Value) / 12) + 1);

            Console.WriteLine(" OctaveOffset = " + OctaveOffset + " div 12 = " + (OctaveOffset / 12));


            // Reset the Labels before adjusting their position
            OCLabel0.Location = OC0pos;
            OCLabel1.Location = OC1pos;
            OCLabel2.Location = OC2pos;
            // Reset Visibility
            OCLabel0.Visible = true;
            OCLabel2.Visible = true;


            // Adjust the Label Position based on the SemiTone Offset
            //if ((int)SetSemitone.Value > 0 && (int)SetSemitone.Value < 12)
            if ((int)SetSemitone.Value > 0 && (int)SetSemitone.Value < 12)
            {
                // It will go beyond the edge right away, hide it
                OCLabel0.Visible = false;

                // Shift the Labels to the LEFT
                for (int i = 0; i < (int)SetSemitone.Value; i++)
                {
                    if (i == 0 || i == 7)
                    {
                        // Move the labels by 25
                        OCLabel0.Location = new Point(OCLabel0.Location.X - 50, OCLabel0.Location.Y);
                        OCLabel1.Location = new Point(OCLabel1.Location.X - 50, OCLabel1.Location.Y);
                        OCLabel2.Location = new Point(OCLabel2.Location.X - 50, OCLabel2.Location.Y);
                    }
                    else
                    {
                        // Move the labels by 50 (because a black key is skipped)
                        OCLabel0.Location = new Point(OCLabel0.Location.X - 25, OCLabel0.Location.Y);
                        OCLabel1.Location = new Point(OCLabel1.Location.X - 25, OCLabel1.Location.Y);
                        OCLabel2.Location = new Point(OCLabel2.Location.X - 25, OCLabel2.Location.Y);
                    }
                }

            }
            //else if ((int)SetSemitone.Value < 0 && (int)SetSemitone.Value > -12)
            else if ((int)SetSemitone.Value < 0 && (int)SetSemitone.Value > -12)
            {
                // If it went beyone the edge, Hide it
                if ((int)SetSemitone.Value < -4)
                    OCLabel2.Visible = false;

                // Adjust the Labels for Negative Semitone offset
                // Shift the labels to the RIGHT
                for (int i = 0; i > (int)SetSemitone.Value; i--)
                {
                    if (i == -4)
                    {
                        // Move the labels by 25
                        OCLabel0.Location = new Point(OCLabel0.Location.X + 50, OCLabel0.Location.Y);
                        OCLabel1.Location = new Point(OCLabel1.Location.X + 50, OCLabel1.Location.Y);
                        OCLabel2.Location = new Point(OCLabel2.Location.X + 50, OCLabel2.Location.Y);
                    }
                    else
                    {
                        // Move the labels by 50 (because a black key is skipped)
                        OCLabel0.Location = new Point(OCLabel0.Location.X + 25, OCLabel0.Location.Y);
                        OCLabel1.Location = new Point(OCLabel1.Location.X + 25, OCLabel1.Location.Y);
                        OCLabel2.Location = new Point(OCLabel2.Location.X + 25, OCLabel2.Location.Y);
                    }
                }
            }
            
            // Reset the focus
            // Must be done or error sounds play when pressing keyboard
            PianoPanel1.Focus();

        }

        // Handles when the Semitone is changes
        // Steps up to the NEXT Octave Every 12
        // Calls SetOctave to make other Adjustments
        private void SetSemitone_ValueChanged(object sender, EventArgs e)
        {
            // Check if Octave has reached it's Max or Min
            if (SetOctave.Value != SetOctave.Maximum && SetOctave.Value != SetOctave.Minimum)
            {
                // If the Semitone Offset Reaches the Max or Min, Adjust the Octave and Reset Semitone
                if (SetSemitone.Value == 12 || SetSemitone.Value == -12)
                {
                    if (SetSemitone.Value == 12)
                    {
                        // Disconnect Event Handler so this event isn't Triggered
                        this.SetSemitone.ValueChanged -= SetSemitone_ValueChanged;
                        SetSemitone.Value = 0;
                        this.SetSemitone.ValueChanged += SetSemitone_ValueChanged;

                        // Adjust Octave, Keep it below or equal to 7
                        if (SetOctave.Value + 1 <= 7)
                            SetOctave.Value += 1;

                    }
                    else if (SetSemitone.Value == -12)
                    {
                        // Disconnect Event Handler so this event isn't Triggered
                        this.SetSemitone.ValueChanged -= SetSemitone_ValueChanged;
                        SetSemitone.Value = 0;
                        this.SetSemitone.ValueChanged += SetSemitone_ValueChanged;

                        // Adjust Octave, Keep it above or equal to 1
                        if (SetOctave.Value - 1 >= 1)
                            SetOctave.Value -= 1;
                    }
                }
                else
                {
                    // Call SetOctave to handle the change
                    SetOctave_ValueChanged(this, null);
                }
            }
            else if (SetSemitone.Value != 0)
            {
                // If Octave is MAX/MIN, we do NOT allow Semitone to Change
                // Disconnect Event Handler so this event isn't Triggered
                this.SetSemitone.ValueChanged -= SetSemitone_ValueChanged;
                SetSemitone.Value = 0;
                this.SetSemitone.ValueChanged += SetSemitone_ValueChanged;

                // Call SetOctave to handle the change
                SetOctave_ValueChanged(this, null);
            }
        }

        #endregion

        #region Keyboard Input to Sound Output Event Handler

        // This Controls the Key Press Input for the Keyboard
        // When the form window is selected
        // If the user presses certain keys
        // Sounds will play
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // If the key is pressed, Call the corresponding Mouse Down Event
                if (e.KeyCode == Keys.Z) { piano_key0_MouseDown(this, null); }
                if (e.KeyCode == Keys.S) { piano_key1_MouseDown(this, null); }
                if (e.KeyCode == Keys.X) { piano_key2_MouseDown(this, null); }
                if (e.KeyCode == Keys.D) { piano_key3_MouseDown(this, null); }
                if (e.KeyCode == Keys.C) { piano_key4_MouseDown(this, null); }
                if (e.KeyCode == Keys.V) { piano_key5_MouseDown(this, null); }
                if (e.KeyCode == Keys.G) { piano_key6_MouseDown(this, null); }
                if (e.KeyCode == Keys.B) { piano_key7_MouseDown(this, null); }
                if (e.KeyCode == Keys.H) { piano_key8_MouseDown(this, null); }
                if (e.KeyCode == Keys.N) { piano_key9_MouseDown(this, null); }
                if (e.KeyCode == Keys.J) { piano_key10_MouseDown(this, null); }
                if (e.KeyCode == Keys.M) { piano_key11_MouseDown(this, null); }
                if (e.KeyCode == Keys.Q) { piano_key12_MouseDown(this, null); }
                if (e.KeyCode == Keys.D2) { piano_key13_MouseDown(this, null); }
                if (e.KeyCode == Keys.W) { piano_key14_MouseDown(this, null); }
                if (e.KeyCode == Keys.D3) { piano_key15_MouseDown(this, null); }
                if (e.KeyCode == Keys.E) { piano_key16_MouseDown(this, null); }
                if (e.KeyCode == Keys.R) { piano_key17_MouseDown(this, null); }
                if (e.KeyCode == Keys.D5) { piano_key18_MouseDown(this, null); }
                if (e.KeyCode == Keys.T) { piano_key19_MouseDown(this, null); }
                if (e.KeyCode == Keys.D6) { piano_key20_MouseDown(this, null); }
                if (e.KeyCode == Keys.Y) { piano_key21_MouseDown(this, null); }
                if (e.KeyCode == Keys.D7) { piano_key22_MouseDown(this, null); }
                if (e.KeyCode == Keys.U) { piano_key23_MouseDown(this, null); }
                if (e.KeyCode == Keys.I) { piano_key24_MouseDown(this, null); }
                if (e.KeyCode == Keys.D9) { piano_key25_MouseDown(this, null); }
                if (e.KeyCode == Keys.O) { piano_key26_MouseDown(this, null); }
                if (e.KeyCode == Keys.D0) { piano_key27_MouseDown(this, null); }
                if (e.KeyCode == Keys.P) { piano_key28_MouseDown(this, null); }
            }
            catch(Exception ex)
            {
                // Show Error
                Console.WriteLine("Key Press Excp : " + ex);
            }
        }

        // Handles when the User Releases a key
        // Calls the mouse up function to stop the sound if it is playing
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                // If the key is released, Call the corresponding Mouse Up Event
                if (e.KeyCode == Keys.Z) { piano_key0_MouseUp(this, null); }
                if (e.KeyCode == Keys.S) { piano_key1_MouseUp(this, null); }
                if (e.KeyCode == Keys.X) { piano_key2_MouseUp(this, null); }
                if (e.KeyCode == Keys.D) { piano_key3_MouseUp(this, null); }
                if (e.KeyCode == Keys.C) { piano_key4_MouseUp(this, null); }
                if (e.KeyCode == Keys.V) { piano_key5_MouseUp(this, null); }
                if (e.KeyCode == Keys.G) { piano_key6_MouseUp(this, null); }
                if (e.KeyCode == Keys.B) { piano_key7_MouseUp(this, null); }
                if (e.KeyCode == Keys.H) { piano_key8_MouseUp(this, null); }
                if (e.KeyCode == Keys.N) { piano_key9_MouseUp(this, null); }
                if (e.KeyCode == Keys.J) { piano_key10_MouseUp(this, null); }
                if (e.KeyCode == Keys.M) { piano_key11_MouseUp(this, null); }
                if (e.KeyCode == Keys.Q) { piano_key12_MouseUp(this, null); }
                if (e.KeyCode == Keys.D2) { piano_key13_MouseUp(this, null); }
                if (e.KeyCode == Keys.W) { piano_key14_MouseUp(this, null); }
                if (e.KeyCode == Keys.D3) { piano_key15_MouseUp(this, null); }
                if (e.KeyCode == Keys.E) { piano_key16_MouseUp(this, null); }
                if (e.KeyCode == Keys.R) { piano_key17_MouseUp(this, null); }
                if (e.KeyCode == Keys.D5) { piano_key18_MouseUp(this, null); }
                if (e.KeyCode == Keys.T) { piano_key19_MouseUp(this, null); }
                if (e.KeyCode == Keys.D6) { piano_key20_MouseUp(this, null); }
                if (e.KeyCode == Keys.Y) { piano_key21_MouseUp(this, null); }
                if (e.KeyCode == Keys.D7) { piano_key22_MouseUp(this, null); }
                if (e.KeyCode == Keys.U) { piano_key23_MouseUp(this, null); }
                if (e.KeyCode == Keys.I) { piano_key24_MouseUp(this, null); }
                if (e.KeyCode == Keys.D9) { piano_key25_MouseUp(this, null); }
                if (e.KeyCode == Keys.O) { piano_key26_MouseUp(this, null); }
                if (e.KeyCode == Keys.D0) { piano_key27_MouseUp(this, null); }
                if (e.KeyCode == Keys.P) { piano_key28_MouseUp(this, null); }
            }
            catch (Exception ex)
            {
                // Show Error
                Console.WriteLine("Key Press Excp : " + ex);
            }
        }

        #endregion

        #region Piano Key Click Event Handlers
        // WARNING - WARNING 
        // Almost ALL of this code is exactly the same
        // Apart from Black Keys and White Keys
        // There are 29 x 2 = 58 event handlers in this section

        // 0
        // Handles when the user Clicks DOWN on the "piano key"
        // Sound will stop when the user releases the mouse button
        private void piano_key0_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing
            if (!isPlaying0)
            {
                //Console.WriteLine("MouseDown1 - Event");
                isPlaying0 = true;

                // Get the Sound file and Start it using the player
                gpl_0 = M_wav(NoteFreq[kIndex[0]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing
                piano_key0.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"
        // Sound will stop and reset the "piano key"
        private void piano_key0_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already
            if (isPlaying0)
            {
                // Reset the Key Color
                piano_key0.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound
                gpl_0.controls.stop();
                gpl_0.URL = "";

                // Reset the Flag
                isPlaying0 = false;
            }
        }

        // 1
        // Handles when the user Clicks DOWN on the "piano key"
        // Sound will stop when the user releases the mouse button
        private void piano_key1_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing
            if (!isPlaying1)
            {
                //Console.WriteLine("MouseDown1 - Event");
                isPlaying1 = true;

                // Get the Sound file and Start it using the player
                gpl_1 = M_wav(NoteFreq[kIndex[1]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing
                piano_key1.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"
        // Sound will stop and reset the "piano key"
        private void piano_key1_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already
            if (isPlaying1)
            {
                // Reset the Key Color
                piano_key1.BackColor = System.Drawing.Color.Black;

                // Stop the Sound
                gpl_1.controls.stop();
                gpl_1.URL = "";

                // Reset the Flag
                isPlaying1 = false;
            }
        }

        // 2
        // Handles when the user Clicks DOWN on the "piano key"
        // Sound will stop when the user releases the mouse button
        private void piano_key2_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing
            if (!isPlaying2)
            {
                //Console.WriteLine("MouseDown2 - Event");
                isPlaying2 = true;
                
                // Get the Sound file and Start it using the player
                gpl_2 = M_wav(NoteFreq[kIndex[2]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing
                piano_key2.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"
        // Sound will stop and reset the "piano key"
        private void piano_key2_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already
            if (isPlaying2)
            {
                // Reset the Key Color
                piano_key2.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound
                gpl_2.controls.stop();
                gpl_2.URL = "";

                // Reset the Flag
                isPlaying2 = false;
            }
        }


        // 3
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key3_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying3)
            {
                //Console.WriteLine("MouseDown3 - Event");                                      
                isPlaying3 = true;

                // Get the Sound file and Start it using the player                             
                gpl_3 = M_wav(NoteFreq[kIndex[3]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key3.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key3_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying3)
            {
                // Reset the Key Color                                                          
                piano_key3.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_3.controls.stop();
                gpl_3.URL = "";

                // Reset the Flag                                                               
                isPlaying3 = false;
            }
        }

        // 4
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key4_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying4)
            {
                //Console.WriteLine("MouseDown4 - Event");                                      
                isPlaying4 = true;

                // Get the Sound file and Start it using the player                             
                gpl_4 = M_wav(NoteFreq[kIndex[4]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key4.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key4_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying4)
            {
                // Reset the Key Color                                                          
                piano_key4.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_4.controls.stop();
                gpl_4.URL = "";

                // Reset the Flag                                                               
                isPlaying4 = false;
            }
        }

        // 5
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key5_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying5)
            {
                //Console.WriteLine("MouseDown5 - Event");                                      
                isPlaying5 = true;

                // Get the Sound file and Start it using the player                             
                gpl_5 = M_wav(NoteFreq[kIndex[5]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key5.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key5_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying5)
            {
                // Reset the Key Color                                                          
                piano_key5.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_5.controls.stop();
                gpl_5.URL = "";

                // Reset the Flag                                                               
                isPlaying5 = false;
            }
        }

        // 6
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key6_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying6)
            {
                //Console.WriteLine("MouseDown6 - Event");                                      
                isPlaying6 = true;

                // Get the Sound file and Start it using the player                             
                gpl_6 = M_wav(NoteFreq[kIndex[6]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key6.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key6_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying6)
            {
                // Reset the Key Color                                                          
                piano_key6.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_6.controls.stop();
                gpl_6.URL = "";

                // Reset the Flag                                                               
                isPlaying6 = false;
            }
        }

        // 7
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key7_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying7)
            {
                //Console.WriteLine("MouseDown7 - Event");                                      
                isPlaying7 = true;

                // Get the Sound file and Start it using the player                             
                gpl_7 = M_wav(NoteFreq[kIndex[7]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key7.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key7_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying7)
            {
                // Reset the Key Color                                                          
                piano_key7.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_7.controls.stop();
                gpl_7.URL = "";

                // Reset the Flag                                                               
                isPlaying7 = false;
            }
        }

        // 8
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key8_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying8)
            {
                //Console.WriteLine("MouseDown8 - Event");                                      
                isPlaying8 = true;

                // Get the Sound file and Start it using the player                             
                gpl_8 = M_wav(NoteFreq[kIndex[8]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key8.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key8_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying8)
            {
                // Reset the Key Color                                                          
                piano_key8.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_8.controls.stop();
                gpl_8.URL = "";

                // Reset the Flag                                                               
                isPlaying8 = false;
            }
        }

        // 9
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key9_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying9)
            {
                //Console.WriteLine("MouseDown9 - Event");                                      
                isPlaying9 = true;

                // Get the Sound file and Start it using the player                             
                gpl_9 = M_wav(NoteFreq[kIndex[9]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key9.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key9_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying9)
            {
                // Reset the Key Color                                                          
                piano_key9.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_9.controls.stop();
                gpl_9.URL = "";

                // Reset the Flag                                                               
                isPlaying9 = false;
            }
        }

        // 10
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key10_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying10)
            {
                //Console.WriteLine("MouseDown10 - Event");                                      
                isPlaying10 = true;

                // Get the Sound file and Start it using the player                             
                gpl_10 = M_wav(NoteFreq[kIndex[10]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key10.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key10_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying10)
            {
                // Reset the Key Color                                                          
                piano_key10.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_10.controls.stop();
                gpl_10.URL = "";

                // Reset the Flag                                                               
                isPlaying10 = false;
            }
        }

        // 11
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key11_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying11)
            {
                //Console.WriteLine("MouseDown11 - Event");                                      
                isPlaying11 = true;

                // Get the Sound file and Start it using the player                             
                gpl_11 = M_wav(NoteFreq[kIndex[11]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key11.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key11_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying11)
            {
                // Reset the Key Color                                                          
                piano_key11.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_11.controls.stop();
                gpl_11.URL = "";

                // Reset the Flag                                                               
                isPlaying11 = false;
            }
        }

        // 12
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key12_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying12)
            {
                //Console.WriteLine("MouseDown12 - Event");                                      
                isPlaying12 = true;

                // Get the Sound file and Start it using the player                             
                gpl_12 = M_wav(NoteFreq[kIndex[12]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key12.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key12_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying12)
            {
                // Reset the Key Color                                                          
                piano_key12.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_12.controls.stop();
                gpl_12.URL = "";

                // Reset the Flag                                                               
                isPlaying12 = false;
            }
        }

        // 13
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key13_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying13)
            {
                //Console.WriteLine("MouseDown13 - Event");                                      
                isPlaying13 = true;

                // Get the Sound file and Start it using the player                             
                gpl_13 = M_wav(NoteFreq[kIndex[13]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key13.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key13_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying13)
            {
                // Reset the Key Color                                                          
                piano_key13.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_13.controls.stop();
                gpl_13.URL = "";

                // Reset the Flag                                                               
                isPlaying13 = false;
            }
        }

        // 14
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key14_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying14)
            {
                //Console.WriteLine("MouseDown14 - Event");                                      
                isPlaying14 = true;

                // Get the Sound file and Start it using the player                             
                gpl_14 = M_wav(NoteFreq[kIndex[14]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key14.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key14_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying14)
            {
                // Reset the Key Color                                                          
                piano_key14.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_14.controls.stop();
                gpl_14.URL = "";

                // Reset the Flag                                                               
                isPlaying14 = false;
            }
        }

        // 15
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key15_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying15)
            {
                //Console.WriteLine("MouseDown15 - Event");                                      
                isPlaying15 = true;

                // Get the Sound file and Start it using the player                             
                gpl_15 = M_wav(NoteFreq[kIndex[15]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key15.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key15_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying15)
            {
                // Reset the Key Color                                                          
                piano_key15.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_15.controls.stop();
                gpl_15.URL = "";

                // Reset the Flag                                                               
                isPlaying15 = false;
            }
        }

        // 16
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key16_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying16)
            {
                //Console.WriteLine("MouseDown16 - Event");                                      
                isPlaying16 = true;

                // Get the Sound file and Start it using the player                             
                gpl_16 = M_wav(NoteFreq[kIndex[16]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key16.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key16_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying16)
            {
                // Reset the Key Color                                                          
                piano_key16.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_16.controls.stop();
                gpl_16.URL = "";

                // Reset the Flag                                                               
                isPlaying16 = false;
            }
        }

        // 17
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key17_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying17)
            {
                //Console.WriteLine("MouseDown17 - Event");                                      
                isPlaying17 = true;

                // Get the Sound file and Start it using the player                             
                gpl_17 = M_wav(NoteFreq[kIndex[17]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key17.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key17_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying17)
            {
                // Reset the Key Color                                                          
                piano_key17.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_17.controls.stop();
                gpl_17.URL = "";

                // Reset the Flag                                                               
                isPlaying17 = false;
            }
        }

        // 18
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key18_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying18)
            {
                //Console.WriteLine("MouseDown18 - Event");                                      
                isPlaying18 = true;

                // Get the Sound file and Start it using the player                             
                gpl_18 = M_wav(NoteFreq[kIndex[18]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key18.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key18_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying18)
            {
                // Reset the Key Color                                                          
                piano_key18.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_18.controls.stop();
                gpl_18.URL = "";

                // Reset the Flag                                                               
                isPlaying18 = false;
            }
        }

        // 19
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key19_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying19)
            {
                //Console.WriteLine("MouseDown19 - Event");                                      
                isPlaying19 = true;

                // Get the Sound file and Start it using the player                             
                gpl_19 = M_wav(NoteFreq[kIndex[19]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key19.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key19_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying19)
            {
                // Reset the Key Color                                                          
                piano_key19.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_19.controls.stop();
                gpl_19.URL = "";

                // Reset the Flag                                                               
                isPlaying19 = false;
            }
        }

        // 20
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key20_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying20)
            {
                //Console.WriteLine("MouseDown20 - Event");                                      
                isPlaying20 = true;

                // Get the Sound file and Start it using the player                             
                gpl_20 = M_wav(NoteFreq[kIndex[20]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key20.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key20_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying20)
            {
                // Reset the Key Color                                                          
                piano_key20.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_20.controls.stop();
                gpl_20.URL = "";

                // Reset the Flag                                                               
                isPlaying20 = false;
            }
        }

        // 21
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key21_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying21)
            {
                //Console.WriteLine("MouseDown21 - Event");                                      
                isPlaying21 = true;

                // Get the Sound file and Start it using the player                             
                gpl_21 = M_wav(NoteFreq[kIndex[21]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key21.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key21_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying21)
            {
                // Reset the Key Color                                                          
                piano_key21.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_21.controls.stop();
                gpl_21.URL = "";

                // Reset the Flag                                                               
                isPlaying21 = false;
            }
        }

        // 22
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key22_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying22)
            {
                //Console.WriteLine("MouseDown22 - Event");                                      
                isPlaying22 = true;

                // Get the Sound file and Start it using the player                             
                gpl_22 = M_wav(NoteFreq[kIndex[22]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key22.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key22_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying22)
            {
                // Reset the Key Color                                                          
                piano_key22.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_22.controls.stop();
                gpl_22.URL = "";

                // Reset the Flag                                                               
                isPlaying22 = false;
            }
        }

        // 23
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key23_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying23)
            {
                //Console.WriteLine("MouseDown23 - Event");                                      
                isPlaying23 = true;

                // Get the Sound file and Start it using the player                             
                gpl_23 = M_wav(NoteFreq[kIndex[23]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key23.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key23_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying23)
            {
                // Reset the Key Color                                                          
                piano_key23.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_23.controls.stop();
                gpl_23.URL = "";

                // Reset the Flag                                                               
                isPlaying23 = false;
            }
        }

        // 24
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key24_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying24)
            {
                //Console.WriteLine("MouseDown24 - Event");                                      
                isPlaying24 = true;

                // Get the Sound file and Start it using the player                             
                gpl_24 = M_wav(NoteFreq[kIndex[24]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key24.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key24_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying24)
            {
                // Reset the Key Color                                                          
                piano_key24.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_24.controls.stop();
                gpl_24.URL = "";

                // Reset the Flag                                                               
                isPlaying24 = false;
            }
        }

        // 25
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key25_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying25)
            {
                //Console.WriteLine("MouseDown25 - Event");                                      
                isPlaying25 = true;

                // Get the Sound file and Start it using the player                             
                gpl_25 = M_wav(NoteFreq[kIndex[25]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key25.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key25_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying25)
            {
                // Reset the Key Color                                                          
                piano_key25.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_25.controls.stop();
                gpl_25.URL = "";

                // Reset the Flag                                                               
                isPlaying25 = false;
            }
        }

        // 26
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key26_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying26)
            {
                //Console.WriteLine("MouseDown26 - Event");                                      
                isPlaying26 = true;

                // Get the Sound file and Start it using the player                             
                gpl_26 = M_wav(NoteFreq[kIndex[26]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key26.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key26_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying26)
            {
                // Reset the Key Color                                                          
                piano_key26.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_26.controls.stop();
                gpl_26.URL = "";

                // Reset the Flag                                                               
                isPlaying26 = false;
            }
        }

        // 27
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key27_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying27)
            {
                //Console.WriteLine("MouseDown27 - Event");                                      
                isPlaying27 = true;

                // Get the Sound file and Start it using the player                             
                gpl_27 = M_wav(NoteFreq[kIndex[27]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key27.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key27_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying27)
            {
                // Reset the Key Color                                                          
                piano_key27.BackColor = System.Drawing.Color.Black;

                // Stop the Sound                                                               
                gpl_27.controls.stop();
                gpl_27.URL = "";

                // Reset the Flag                                                               
                isPlaying27 = false;
            }
        }

        // 28
        // Handles when the user Clicks DOWN on the "piano key"                                
        // Sound will stop when the user releases the mouse button                             
        private void piano_key28_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the key is already playing                                              
            if (!isPlaying28)
            {
                //Console.WriteLine("MouseDown28 - Event");                                      
                isPlaying28 = true;

                // Get the Sound file and Start it using the player                             
                gpl_28 = M_wav(NoteFreq[kIndex[28]], gDuration, gVolume);

                // Change the color of the key to indicate it is playing                        
                piano_key28.BackColor = Color.FromArgb(gPressColor, gPressColor, gPressColor);
            }
        }

        // Handles when the user Releases the mouse button on the "piano key"                  
        // Sound will stop and reset the "piano key"                                           
        private void piano_key28_MouseUp(object sender, MouseEventArgs e)
        {
            // If it IS playing already                                                         
            if (isPlaying28)
            {
                // Reset the Key Color                                                          
                piano_key28.BackColor = Color.FromArgb(255, 255, 255);

                // Stop the Sound                                                               
                gpl_28.controls.stop();
                gpl_28.URL = "";

                // Reset the Flag                                                               
                isPlaying28 = false;
            }
        }








        #endregion

        #region Sound Playing Functions

        //public static WindowsMediaPlayer M_wav(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        public static WindowsMediaPlayer M_wav(Double frequency, int msDuration, UInt16 volume = 16383)
        {
            //Console.WriteLine(" Make Wave Called ");
            
            // Create the Output File name, using the frequency, volume and duration
            String fname = @"PianoAudio\Sine_frq_" + Math.Floor(frequency) + "_vol_" + volume + "_dur_" + msDuration + ".wav";

            //Console.WriteLine(" Trying to make file stream for file: " + fname);

            // Check if the file exists - if it doesnt, Make the file
            if (!File.Exists(fname))
            {
                // If the directory doesnt already Exist, create it
                if (!Directory.Exists(fname))
                    Directory.CreateDirectory(Path.GetDirectoryName(fname));

                // Create the file to write to
                // and a binary writer to write the file
                FileStream mStrm = new FileStream(fname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                BinaryWriter writer = new BinaryWriter(mStrm);

                try
                {
                    // Create each part of the Wave File
                    // According to the Wave file format
                    const double TAU = 2 * Math.PI;
                    int formatChunkSize = 16;
                    int headerSize = 8;
                    short formatType = 1;
                    short tracks = 1;
                    int samplesPerSecond = 44100;
                    short bitsPerSample = 16;
                    short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
                    int bytesPerSecond = samplesPerSecond * frameSize;
                    int waveSize = 4;
                    int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
                    int dataChunkSize = samples * frameSize;
                    int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;

                    Console.WriteLine(" Starting to Write! ");

                    // Begin Writing the Wave file
                    // Header Section
                    // var encoding = new System.Text.UTF8Encoding();
                    writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
                    writer.Write(fileSize);
                    writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
                    writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
                    writer.Write(formatChunkSize);
                    writer.Write(formatType);
                    writer.Write(tracks);
                    writer.Write(samplesPerSecond);
                    writer.Write(bytesPerSecond);
                    writer.Write(frameSize);
                    writer.Write(bitsPerSample);
                    writer.Write(0x61746164); // = encoding.GetBytes("data")

                    // Loop to write the Sine Wave into the file
                    writer.Write(dataChunkSize);
                    {
                        double theta = frequency * TAU / (double)samplesPerSecond;
                        // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                        // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                        double amp = volume >> 2; // so we simply set amp = volume / 2

                        // Loop to write the Sine Wave
                        for (int step = 0; step < samples; step++)
                        {
                            short s = (short)(amp * Math.Sin(theta * (double)step));
                            //Console.WriteLine("Value of S = " + s);
                            writer.Write(s);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("error E :" + e);
                }
                finally
                {
                    // Close the Writer and File Stream
                    writer.Close();
                    mStrm.Close();
                }
            }
            
            // Create a windows Media player variable to return
            // with the sound file
            var pl_1 = new WMPLib.WindowsMediaPlayer();

            // Try to give the the media player the sound file
            try
            {
                pl_1.URL = fname; // give it the file name

                //Audio temp = new Audio("bell.mp3");
            }
            catch(Exception e)
            {
                // Show the exception
                Console.WriteLine("Audio Player Failed : " + e);
            }
            // Return the player
            return pl_1;
        }

        #endregion

        #region Other Button Events

        // Exit the Program
        private void ExitButton_Click(object sender, EventArgs e)
        {
            // Exit 
            Application.Exit();
        }

        // Used to RESET all the Option Controls
        // All Options Controls will be Set to Default Values
        private void ResetButton_Click(object sender, EventArgs e)
        {
            // Reset all of the Options to Default Values 
            VolumeBar1.Value = 9;
            SetOctave.Value = 5;
            SetSemitone.Value = 0;
            SetNoteLength.Value = 6.0M;

        }

        // Used to Hide the Splash Panel
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            PianoSplash1.Visible = false;
        }

        #endregion
    }
}
