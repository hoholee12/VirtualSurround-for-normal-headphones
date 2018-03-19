# VirtualSurround-for-normal-headphones
no more surround sound gaming headphones needed, all you need is a EqualizerAPO(https://sourceforge.net/projects/equalizerapo/) and this config.txt! <br/>
 <br/>
all handtuned by myself for hours.. <br/>
 <br/>
you can test if this virtual surround works with these links:  
https://www.youtube.com/watch?v=PqVCPE8_ntE - dolby 5.1 video  <br/>
https://www.youtube.com/watch?v=_ttYSpt8oiw - some other 5.1 video  <br/>
 <br/>
this was originally intended to simulate ECHO effect on beatmania IIDX. <br/>
i now have figured out how to do proper surround and delay. <br/>
it now sounds 90% close to the original! <br/>
 <br/>
formula: <br/>
ECHO = DELAY(5ms) + SURROUND <br/>
REVERB = SURROUND <br/>
EQ ONLY = minimal SURROUND <br/>
COMPRESSOR = DELAY(0.5ms) + minimal SURROUND <br/>
REVERB EX = SURROUND x2 <br/>
 <br/>
