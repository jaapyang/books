#! /usr/bin/env python3
# -*-coding:utf-8-*-
import os
import re

import requests
from bs4 import BeautifulSoup

LIST_URL = 'http://www.23us.so/files/article/html/14/14741/index.html'
SAVE_DIR = 'D:\\download_books'


class Charector():
    def __init__(self, url, title):
        self.Href = url
        self.Title = title


def get_page_html(url):
    """获取指定URL页面的HTML"""
    response = requests.get(url)
    return response.content.decode('utf-8')


def parse_list(menu_html):
    """获取小说目录列表"""
    soup = BeautifulSoup(menu_html)
    link_list = soup.find(id='at').find_all('a')
    detail_url_list = []
    for link in link_list:
        c = Charector(link.attrs['href'], link.text)
        detail_url_list.append(c)
    return detail_url_list


def parse_detail_page_content(page_content):
    """
    解析小说页内容
    """
    soup = BeautifulSoup(page_content)
    content = soup.find(id='contents').text
    return content


def read_txt_content_by_line(path):
    with open(path, 'r', encoding='utf-8') as f:
        return f.readlines()


if __name__ == '__main__':
    lines = read_txt_content_by_line(os.path.join(SAVE_DIR, '超维术士.txt'))
    for line in lines:
        print(line)
    # menu_html = get_page_html(LIST_URL)
    # charact_list = parse_list(menu_html)
    # with open(os.path.join(SAVE_DIR, '14741.txt'), 'w', encoding='utf-8') as f:
    #     for item in charact_list:
    #         print(item.Href + '/' + item.Title)
    #         page_html = get_page_html(item.Href)
    #         page_content = parse_detail_page_content(page_html)
    #         f.write(item.Title)
    #         f.write(page_content)

        # html = get_page_html('http://www.23us.so/files/article/html/14/14741/9528946.html')
        # content = parse_detail_page_content(html)
        # save_to_file('01 test',content)
