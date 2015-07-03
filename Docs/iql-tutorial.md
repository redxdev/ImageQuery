# IQL Tutorial

Welcome to ImageQuery! This document will walk you through using ImageQuery's query language,
IQL. For this tutorial, we're going to create a simple program that inverts the colors of an
image. Later, we will modify the program to do some other cool manipulations

## Inputs and Outputs

The very first thing you need to do is find yourself an image to use as an input. Most image
formats are supported, and it can be any size. If you don't have an image to use, there are
a few in the "Samples" directory.

Next, create a new IQL file called "invert.iql". The first thing we need in this file is a
list of inputs and outputs. Put the following at the top of the file:

    INPUT in
	OUTPUT out[in.width, in.height]

The first line defines an input canvas called _in_. This will be where we get our input data
from. The second line defines an output, where we will be writing to. The input data will be
defined by the file you chose earlier, but the output data isn't backed by anything. That's
why we have this bracket syntax which defines how large the output canvas is.

Note that keywords such as _INPUT_ and _OUTPUT_ may be all upper case or all lower case. The
IQL parser doesn't care as long as all of the letters in the keyword are the same case.

## Apply and Select

The most common operations in IQL are apply and select. Select statements _select and manipulate_
a set of data from a canvas. Apply statements _apply_ a selection to a canvas.

Let's start by writing a select to get and manipulate data from the input canvas. Don't actually
put this in your file as it is invalid IQL since it doesn't actually do anything with the data.

    SELECT {r, g, b, a} FROM in

This is a (pointless) select statement that selects all pixels from the _in_ canvas. The
manipulation doesn't actually do anything, as it simply passes the components of each pixel through
without modifying them. You could actually shorten this select to the following:

    SELECT color FROM in

_color_ is defined whenever you are in a select and is set to the color of the current pixel.

Let's create the inversion select:

    SELECT {1-r,1-g,1-b,a} FROM in

Color components in IQL are between the values of 0 and 1 (and clamped as such), so we subtract
each component of each pixel from 1 (aside from the alpha: we want that to stay the same).

### Apply the Selection

We need to actually make the data we selected go somewhere. The apply statement does just that.

    out: SELECT {1-r,1-g,1-b,a} FROM in

Here, we are saying "Apply this selection to the _out_ canvas". Any pixel selected will replace
the corresponding pixel in the _out_ canvas (we'll talk about how you can modify pixel coordinates
later).

That's it! Here's the full source of the IQL file:

    # Invert colors
	INPUT in
	OUTPUT out[in.width, in.height]
	out: SELECT {1-r,1-g,1-b,a} FROM in

You can run this through IQM with the following (replace the input, output, and iql files as needed):

    iqm -i in input.png -o out output.png invert.iql

## Complex Selects

There's one major aspect of select we haven't talked about: The _WHERE_ clause! Using _WHERE_ in a select
allows you to only work with certain pixels. Try using this code:

    INPUT in
	OUTPUT out[in.width, in.height]
	out: SELECT color FROM in WHERE x % 0 == 5 AND y % 0 == 5

Running this through IQM should result in a grid of colored dots that resemble your input image. The
_WHERE_ clause takes an expression that evaluates to a boolean. In this case, it is an _AND_ expression,
which only results in _true_ when both sides of the expression are also _true_. Each side of the expression
takes a component of the pixel coordinate (x or y) and does a _modulus_ operation on it, then comparing
the result to the number _5_. This is a roundabout way of saying that only pixels that have a coordinate with
both components divisible by 5 will be selected.

The black "grid" in the output is due to ImageQuery setting every pixel to have a default color of black with
an alpha of 1.

## Complex Applys

The last bit this tutorial is going to go over is complex apply statements. You can do a lot with a normal
apply and select, but what about when you want to modify which coordinates each pixel is going to? The answer
to that is the complex apply statement. We're going to make an IQL script that rotates an image 90 degrees:

    INPUT in
	OUTPUT out[in.height, in.width]
	
We're rotating the image 90 degrees, so the coordinates are switched. As a result, the output canvas' dimensions
are the opposite of the input canvas'.

    out[y,x]: SELECT color FROM in

Here we've added some brackets around two expressions in the apply statement. These brackets should look familiar:
they work similarly to the way you define dimensions for output canvases. The difference is that it is expecting
coordinate expressions instead of a width and height. For each selected pixel, the expression is run and the result
is used as the coordinate when the pixel is applied to the canvas. You can also access the usual variables (r, g,
b, a, color, etc...) from within the coordinate expressions, but do note that you are within _out's_ context in this
case: _color_ refers to the color in _out_ and not the color you selected.