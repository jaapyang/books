#! /usr/bin/env python3
# -*-coding:utf-8-*-
import json
import random

import requests
from bs4 import BeautifulSoup

from Dtos.book_model import BookInfo, MenuItemInfo

headers_list = [
    {
        "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"},
]


def random_get_the_first_from_list(data_list):
    random.shuffle(data_list)
    data = data_list[0]
    # print(data)
    return data


class BookDownLoadHandler:
    def __init__(self):
        pass

    def handler_menu_info(self, menu_url):
        """获取已经转换成JSON格式的目录信息"""
        book_info = self.__get_menu_list(menu_url)
        json_str = json.dumps(book_info.__dict__, default=lambda x: x.__dict__)
        return json_str

    def __get_menu_list(self, menu_url):
        """获取目录列表信息集合"""
        menu_html = self.__get_page_html(menu_url)
        menu_item_list = self.__parse_list(menu_html=menu_html)

        book_info = BookInfo(book_name="test",
                             menu_url=menu_url,
                             menu_list=menu_item_list)

        return book_info

    def __parse_list(self, menu_html):
        """获取小说目录列表"""
        soup = BeautifulSoup(menu_html)
        link_list = soup.find(id='at').find_all('a')
        detail_url_list = []
        sort_id = 0
        for link in link_list:
            menu_item = MenuItemInfo(url=link.attrs['href'],
                                     title=link.text,
                                     sortId=sort_id)
            sort_id = sort_id + 1

            detail_url_list.append(menu_item)
        return detail_url_list

    def __get_page_html(self, url):
        """获取指定URL页面的HTML"""
        # proxies = random_get_the_first_from_list(http_proxies_list)
        headers = random_get_the_first_from_list(headers_list)
        with requests.get(url, headers=headers) as response:
            # response = requests.get(url, proxies=proxies, headers=headers)
            return response.content.decode('utf-8')
