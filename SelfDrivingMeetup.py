#!/usr/bin/env python
# coding: utf-8

# In[136]:


import math
import random


# In[137]:


f= open("input.txt","r")
# f= open("sdc_point_cloud.txt","r")
data = f.readlines()
f.close()
p = float(data[0])
N = int(data[1])
points =  [ [ float(x) for x in d.split('\t') ] for d in data[2:]]


# In[138]:


def point_h(x):
    err = 0
    for p in points:
        val = abs(x[0]*p[0] + x[1]*p[1] + x[2]*p[2]) / (x[0]**2 + x[1]**2+ x[2]**2) 
        err += math.log(val+1)
    
    return err


# In[139]:


def get_plane_by_points(p1, p2, p3):
    x1 = p1[0]
    y1 = p1[1]
    z1 = p1[2]
    x2 = p2[0]
    y2 = p2[1]
    z2 = p2[2]
    x3 = p3[0]
    y3 = p3[1]
    z3 = p3[2]
    A = y1* (z2 - z3) + y2* (z3 - z1) + y3* (z1 - z2) 
    B = z1* (x2 - x3) + z2* (x3 - x1) + z3* (x1 - x2) 
    C = x1* (y2 - y3) + x2* (y3 - y1) + x3* (y1 - y2) 
    D = x1* (y2* z3 - y3* z2) + x2* (y3* z1 - y1* z3) + x3* (y1* z2 - y2* z1)
    D *= -1
    
    return [A,B,C,D]


# In[140]:


planes_count = min(N, 1000 )


# In[141]:


planes = []
for i in range(planes_count):
    pi1 = random.randint(0, (len(points) -1))
    pi2 = random.randint(0, (len(points) -1))
    pi3 = random.randint(0, (len(points) -1))
    while( pi2 == pi1 ):
        pi2 = (pi2+1) %  len(points)
    pi3 = random.randint(0, (len(points) -1))
    while(pi3 == pi1 or pi3 == pi2):
        pi3 = (pi3+1) %  len(points)
    val = 0
    try:
        factors = get_plane_by_points(points[pi1], points[pi2], points[pi3])
        val = point_h(factors)
    except Exception:
        continue
    else:
        planes.append({"factors": factors, "val": val})


# In[142]:


res_plane = min(planes, key=lambda k: k['val']) 


# In[143]:


res = ""
for f in res_plane["factors"]:
    res += "%0.6f " % f
print(res)


# In[ ]:





# In[ ]:




