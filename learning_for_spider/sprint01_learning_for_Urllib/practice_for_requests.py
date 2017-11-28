#! /usr/bin/env python3
# -*-coding:utf-8-*-
import requests
import json

# response = requests.get("http://www.baidu.com")
# print(type(response))
# print(response.status_code)
# print(type(response.text))
# print(response.text)
# print(response.cookies)
# print(response.content)
# print(response.content.decode('utf-8'))

# r = requests.post('http://httpbin.org/post')
# print(r.text)
# r = requests.head('http://httpbin.org/get')
# print(r.text)
# r = requests.delete('http://httpbin.org/delete')
# print(r.text)
# r = requests.options('http://httpbin.org/get')
# print(r.text)
# r = requests.put('http://httpbin.org/put')
# print(r.text)

r = requests.head('http://httpbin.org/get')
print(r.json())
