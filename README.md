# PianoSynth
A simple **Piano Synthesizer** created using a Windows Form Application in C#. 
Play the piano with your QWERTY keyboard, or by clicking the keys on the GUI.

### Screenshot

![](https://i.imgur.com/ADacxdx.png)

### Purpose
This application allows you to play a "Synthesized Piano", it was created as a class project where we were told to make something interesting/useful with a C# form - this is what I chose to make.

### Controls
You can control the piano by clicking the keys **OR** you can use your keyboard to play the piano.

- The ***White*** Keys on the Left start at **ZXCVBNM** then move to **QWERTYUIOP**
- The ***Black*** Keys are controlled by the Keys inbetween each of the keys listed above 
    - **SD, GHJ, 23, 567, 90.**

You can control the **Volume** of the sounds, and you can Also change the 
**Octave** of the Keyboard between:

- C1 which is 32 HZ sine Wave
- C9 which is 8372 HZ sine Wave

The **Max Duration** of the sine wave can also be controlled, and you can reset everything to **default** with the ***Reset button***.


## Note

**Windows Media Player** was required to play **MORE** than 1 Sound at a time, this required the Sine Waves to be output as **Audio Files**.

The Files should only be created if they dont already exist.

(Creating Unique files for every combo of Volume, Pitch, and Length is **definitly a bad idea** - it was just easy to implement at the time. There are some alternatives to windows media player, however they require special drivers/libraries - and would not work unless the OS had them)

**Note:** Mashing too many keys at the same time MAY cause the program to Lag or "Hang", please be gentle.


