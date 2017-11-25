#! /usr/bin/env python3
# -*-coding:utf-8-*-

"""
Urllib是python内置的HTTP请求库
包括以下模块
urllib.request 请求模块
urllib.error 异常处理模块
urllib.parse url解析模块
urllib.robotparser robots.txt解析模块
"""

import urllib.request
import urllib.parse


def request_for_baidu():
    """
    url参数的使用
    GET请求百度，并获取网页内容
    :return: None
    """
    response = urllib.request.urlopen('https://www.baidu.com')
    content = response.read().decode('GBK')
    print(content)


def post_with_data():
    """
    data参数的使用
    这里通过http://httpbin.org/post网站演示（该网站可以作为练习使用urllib的一个站点使用
    可以模拟各种请求操作
    :return:
    b'word=hello'
    {
      "args": {},
      "data": "",
      "files": {},
      "form": {
        "word": "hello"
      },
      "headers": {
        "Accept-Encoding": "identity",
        "Connection": "close",
        "Content-Length": "10",
        "Content-Type": "application/x-www-form-urlencoded",
        "Host": "httpbin.org",
        "User-Agent": "Python-urllib/3.5"
      },
      "json": null,
      "origin": "222.240.107.192",
      "url": "http://httpbin.org/post"
    }
    """
    url = "http://httpbin.org/post"
    data = bytes(urllib.parse.urlencode({"word": "hello"}), encoding='utf8')
    print(data)
    response = urllib.request.urlopen(url, data=data)
    print(response.read().decode('utf-8'))


if __name__ == '__main__':
    # request_for_baidu()
    post_with_data()
