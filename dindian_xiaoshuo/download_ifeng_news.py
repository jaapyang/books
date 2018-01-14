#! /usr/bin/env python3
# -*-coding:utf-8-*-

import logging
import os
import random
import re
import requests
import time
from bs4 import BeautifulSoup

LIST_URL = 'http://news.ifeng.com/listpage/11502/0/1/rtlist.shtml'
SAVE_DIR = 'D:\\download_books'
SLEEP_TIME = 3
http_proxies_list = [
    # {"http": "http://115.28.148.137:8118", "https": "https://112.114.99.133:8118"},
    {"http": "http://106.75.56.87:80", "https": "https://123.53.137.32:48420"},
    # {"http": "http://221.233.85.18:3128", "https": "https://118.72.124.97:80"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
]

headers_list = [
    {
        "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"},
]


def random_get_the_first_from_list(data_list):
    random.shuffle(data_list)
    data = data_list[0]
    # print(data)
    return data


def get_page_html(url):
    """获取指定URL页面的HTML"""
    time.sleep(SLEEP_TIME)
    # proxies = random_get_the_first_from_list(http_proxies_list)
    headers = random_get_the_first_from_list(headers_list)
    with requests.get(url, headers=headers) as response:
        # response = requests.get(url, proxies=proxies, headers=headers)
        return response.content.decode('utf-8')


def get_new_url(page_content):
    soup = BeautifulSoup(page_content)
    news_list = soup.select('.newsList a')
    print(news_list)

if __name__ == '__main__':
    """下载新闻列表"""
    content = get_page_html(LIST_URL)
    list = get_new_url(content)
    for item in list:
        item.attr()