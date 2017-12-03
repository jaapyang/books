#! /usr/bin/env python3
# -*-coding:utf-8-*-
import sys

import requests

if __name__ == '__main__':
    # LIST_URL = input("请输入列表页网址:")
    # print(LIST_URL)
    # file_name = input("请输入保存的文件名:")
    # print(file_name)
    """测试程序"""
    # try:
    response = requests.get("http://www.baidu.com",
                            proxies={
                                "HTTP": "http://119.57.144.253:8080"
                            },
                            headers={
                                "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"},
                            )
    print(response.content.encode('utf-8'))
    # except BaseException as e:
    #     print(e)
