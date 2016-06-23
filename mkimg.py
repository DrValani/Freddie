#!/usr/bin/env python
from PIL import Image
import itertools

import numpy as np

with open("vertices.txt") as vf:
    lines = [line for line in vf.readlines()]
    heights = [[float(height) for height in line.strip().split(",") if height is not ""] for line in lines]

allHeights = list(itertools.chain(*heights))

colors = [
    (27, 17, 25),
    (49, 14, 32),
    (97, 10, 35),
    (193, 0, 29),
    (255, 58, 32),
]



w,h = (len(heights[0]),len(heights))
im = Image.new("RGB", (w,h))
pixels = im.load()

def roundrobin(*iterables):
    "roundrobin('ABC', 'D', 'EF') --> A D E B F C"
    # Recipe credited to George Sakkis
    pending = len(iterables)
    nexts = itertools.cycle(iter(it).next for it in iterables)
    while pending:
        try:
            for next in nexts:
                yield next()
        except StopIteration:
            pending -= 1
            nexts = itertools.cycle(itertools.islice(nexts, pending))

def avg(a,b):
    return int((a + b)/2.0)

def doubleColors(colors):
    # assume black first, white last
    ncols = [(0,0,0)]+colors+[(255,255,255)]
    def blend(a, b):
        return tuple(map(avg, a, b))
    return list(roundrobin(itertools.imap(blend, ncols, ncols[1:]), colors))

def mkHeight2colorPercentiles(heights, colors):
    cols = []
    fraction = (100.0/len(colors))
    for i,col in enumerate(colors):
        cols.append( (np.percentile(heights, (i+1)*fraction), col) )

    def h2c(h):
        for pct, col in cols:
            if h < pct:
                return col
        return (255,0,0)
    return h2c

def mkHeight2colorFixed(heights, colors):
    cols = []
    maxh = max(heights)
    minh = min(heights)
    fraction = (maxh-minh)/len(colors)
    for i,col in enumerate(colors):
        cols.append( (minh+(i+1)*fraction, col) )
    print minh, maxh, cols
    def h2c(h):
        for pct, col in cols:
            if h < pct:
                return col
        return (255,0,0)
    return h2c

height2color = mkHeight2colorFixed(allHeights, doubleColors(doubleColors(colors)))

for y in range(h):
    for x in range(w):
        pixels[x,y] = height2color(heights[y][x])

im.save("test.png")
