# KASM

A high-level assembly-like language.

## How do you use this language?

Read the wiki on the [GitHub](https://github.com/telekiwi/kasm/wiki) page for KASM. It has lots of cool and epic documentation.

## Why does this exist?

KASM exists because I found creating an assembly language in a high level language like C# would be a niche concept, but still a fun idea to implement.  I've made plenty of esolangs in my day, but they've all been;

- hard to use
- completely broken
- high level

To try and shake this esolang up, I decided to create something that *looks* like it's low level, but actually is high level.

## What do you mean by that?

Instead of directly communicating with the computer, this language is interpreted in C#. This not only makes it easier to implement (C# isn't as hard as base assembly), but it's also compatible with anything that supports C# without having to create a million different variations of it to support different hardware.

## How do I get started?

First, make sure your computer can execute C# code. Download .NET Core from Microsoft's website, then open a command prompt and navigate to a folder of your choosing. Then, run;

`git clone https://github.com/TeleKiwi/KASM.git`

Still in the command prompt, navigate to src/main. Then, run;

`csc Program.cs`

An executable should (hopefully) be generated. Run it. Then, when you see the prompt (`> `), paste the directory of your KASM file. (It must end in .kasm, or you'll get an error. Also, don't surround the path with quotation marks.) It should get interpreted!

## Features

(/) - implemented
(-) - not implemented

(/) Stack, RAM, registers
(/) Basic I/O (iout & ioin)
(/) Stack manip (push & pop)
(/) Basic arithmatic (add, sub)
(/) Branching and subroutines (jmp, jsr)
(/) Comments
(/) Inline comments
(/) Looping
(-) Subroutines with arguments
(-) Conditional branching (jmp...if)
(-) Macros (@define test 1)

## Planned shit

Turing completeness (via rule 110)
Self compiling compiler (LMAO)
