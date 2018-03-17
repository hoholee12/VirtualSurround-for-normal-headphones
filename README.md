# VirtualSurround-for-normal-headphones
no more surround sound gaming headphones needed, all you need is a EqualizerAPO(https://sourceforge.net/projects/equalizerapo/) and this config.txt! 
 
all handtuned by myself for hours.. 
 
you can test if this virtual surround works with these links:  
https://www.youtube.com/watch?v=PqVCPE8_ntE - dolby 5.1 video  
https://www.youtube.com/watch?v=_ttYSpt8oiw - some other 5.1 video  
 
this was originally intended to simulate ECHO effect on beatmania IIDX. 
i now have figured out how to do proper surround and delay. 
it now sounds 90% close to the original! 
 
formula: 
ECHO = DELAY(5ms) + SURROUND 
REVERB = SURROUND 
EQ ONLY = DELAY(5ms) 
COMPRESSOR = DELAY(0.5ms) 
