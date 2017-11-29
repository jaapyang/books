#! /usr/bin/env python3
# -*-coding:utf-8-*-
import os
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

def practice_post():
    data = {
        "name": "paul",
        "age": 34
    }
    response = requests.post("http://httbin.org/post", data=data)
    print(response.text)


def practice_upload_file():
    files = {"files": open("Static\\test.txt", "rb")}
    response = requests.post("http://httpbin.org/post", files=files)
    print(response.text)


def take_cookie():
    s = requests.Session()
    s.get("http://httpbin.org/cookies/set/number/234567")
    response = s.get("http://httpbin.org/cookies")
    print(response.text)


def set_proxies():
    """
    这里有问题
    """
    proxies = {
        "http": "http://112.114.97.137:8118",
        "https": "https://171.39.9.40:8123",
    }
    response = requests.get("https://www.baidu.com", proxies=proxies)
    print(response.text)


if __name__ == '__main__':
    # practice_post()
    # practice_upload_file()
    # take_cookie()
    set_proxies()
