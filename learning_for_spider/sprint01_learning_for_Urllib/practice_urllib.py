#! /usr/bin/env python3
# -*-coding:utf-8-*-


import urllib.request

response = urllib.request.urlopen('https://www.baidu.com')
content = response.read().decode('GBK')
print(content)