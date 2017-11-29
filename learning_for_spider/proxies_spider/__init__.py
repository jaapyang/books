#! /usr/bin/env python3
# -*-coding:utf-8-*-
import requests


def get_page_content(url):
    proxies = {
        "http": "http://112.114.97.137:8118",
        "https": "http://110.72.38.185:8123",
    }
    response = requests.get(url, proxies=proxies)
    content = response.content.decode('utf-8')
    return content


if __name__ == '__main__':
    print(get_page_content("http://www.xicidaili.com/nn/"))
