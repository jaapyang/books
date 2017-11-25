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

from urllib import request, parse


def request_for_baidu():
    """
    url参数的使用
    GET请求百度，并获取网页内容
    :return: None
    """
    response = request.urlopen('https://www.baidu.com')
    content = response.read().decode('GBK')
    print(content)


def request_with_timeout():
    try:
        request.urlopen("http://httpbin.org/get", timeout=0.1)
    except Exception as e:
        print(e)


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
    data = bytes(parse.urlencode({"word": "hello"}), encoding='utf8')
    print(data)
    response = request.urlopen(url, data=data)
    print(response.read().decode('utf-8'))


def set_request_headers():
    """
    设置Headers
    给请求添加头部信息，从而定制自己请求网站时的头部信息

    有很多网站为了防止程序爬虫爬网站造成网站瘫痪，
    会需要携带一些headers头部信息才能访问，最长见的有user-agent参数
    :return:
    """
    url = "http://httpbin.org/post"
    headers = {
        'User-Agent': 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)',
        'Host': 'httpbin.org'
    }
    dict_parameters = {
        'name': 'paul'
    }
    data = bytes(parse.urlencode(dict_parameters), encoding='utf8')
    req = request.Request(url=url, data=data, headers=headers, method="POST")
    response = request.urlopen(req)
    print(response.read().decode('utf-8'))


if __name__ == '__main__':
    # request_for_baidu()
    # post_with_data()
    # request_with_timeout()
    set_request_headers()
