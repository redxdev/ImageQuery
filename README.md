# ImageQuery

ImageQuery is a relatively simple programming environment made for image manipulation. It includes
a language known as IQL which is **very** loosely based on SQL syntax along with some concepts from
shading languages.

## Goals

The goals of this project are to create a simple language and programming environment which can do
fast processing of images.

## Concepts

### Types

IQL is a very simple language and only includes the following types: number, color, and canvas.

#### Number

Numbers are floating point numbers, generally 32 bits in length (this may vary depending on the
implementation).

#### Color

Colors contain four numbers labeled R, G, B, and A.

#### Canvas

Canvases are the images themselves. They contain a list of colors, one for each pixel. Canvases
are also marked as being read-only, write-only, or read/write (this will be important later).

### Defining Canvases

Canvases are defined as inputs (read-only), outputs (write-only) or intermediates (read/write).
Inputs are generally loaded from disk by the environment, while outputs are generally written
to disk at the end of execution. Intermediates exist purely within the environment and are used
to hold data during execution.

### Selection

The primary operation in IQL is selection (similar to SQL select statements). Selection is
generally done to retrieve values from a canvas, modify those values, and then write them
back out.

Selection operations are designed to be able to work in parallel with themselves. That is,
the environment can take a single select and break it into jobs. Jobs can then be split across
multiple threads to speed up processing.

### Assignment

The other primary operation in IQL is assignment. This actually sets the values of a canvas. The
left side of an assignment will always be a canvas, while the right side will always be a
selection. Assignments only affect pixels on the canvas that were selected by the selection
operation.

Due to the parallel nature of select operations, select operations cannot modify the canvas they
are selecting from while assigning to said canvas. As a result, assigning to the same canvas you
are selecting from is slower than assigning to a different canvas.