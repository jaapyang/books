#! /usr/bin/env python3
# -*-coding:utf-8-*-
import random

import requests
import time

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
    sleep_time = random.randint(1, 5)
    print(' %s - %d' % (url, sleep_time))
    time.sleep(sleep_time)
    # proxies = random_get_the_first_from_list(http_proxies_list)
    headers = random_get_the_first_from_list(headers_list)

    try:
        with requests.get(url, headers=headers) as response:
            # response = requests.get(url, proxies=proxies, headers=headers)
            return response.content.decode('utf-8')
    except Exception as e:
        print(e)
        return None
