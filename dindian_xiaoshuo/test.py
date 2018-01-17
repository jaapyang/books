#! /usr/bin/env python3
# -*-coding:utf-8-*-
import json
import logging
import sys

import os
import requests

from Dtos.book_model import ChapterInfo
from Handlers.book_download_handler import BookDownLoadHandler
from rabbit_sender import RabbitMqMessageHandler


SAVE_DIR = 'D:\\download_books'

def TestFor_pars_dic():
    dic = [{'Title': '第一千两百二十二章 心情不好', 'SortId': 1878, 'Id': 8009,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9482102.html', 'Content': ''},
           {'Title': '第一千两百二十三章 没有计划', 'SortId': 1879, 'Id': 8010,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9491132.html', 'Content': ''},
           {'Title': '第一千两百二十四章 没有主见', 'SortId': 1880, 'Id': 8011,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9491140.html', 'Content': ''},
           {'Title': '第一千两百二十五章 不太满意', 'SortId': 1881, 'Id': 8012,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9499940.html', 'Content': ''},
           {'Title': '第一千两百二十六章 很不愉快', 'SortId': 1882, 'Id': 8013,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9499945.html', 'Content': ''},
           {'Title': '第一千两百二十七章 习以为常', 'SortId': 1883, 'Id': 8014,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9508193.html', 'Content': ''},
           {'Title': '第一千两百二十八章 没有头绪', 'SortId': 1884, 'Id': 8015,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9508205.html', 'Content': ''},
           {'Title': '第一千两百二十九章 不敢前进', 'SortId': 1885, 'Id': 8016,
            'Url': 'http://www.23us.so/files/article/html/13/13639/9515802.html', 'Content': ''}]

    for item in dic:
        print(item)
    pass


if __name__ == '__main__':
    # LIST_URL = input("请输入列表页网址:")
    # print(LIST_URL)
    # file_name = input("请输入保存的文件名:")
    # print(file_name)
    """测试程序"""
    # try:
    # response = requests.get("http://www.baidu.com",
    #                         proxies={
    #                             "HTTP": "http://119.57.144.253:8080"
    #                         },
    #                         headers={
    #                             "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"},
    #                         )
    # print(response.content.encode('utf-8'))
    # except BaseException as e:
    #     print(e)

    """测试对象转Json"""
    # chapter = ChapterInfo(source_link="http://www.baidu.com",
    #                       chapter_title="chapter 1",
    #                       chapter_content="no body",
    #                       chapter_sortId=112,
    #                       book_name="test book name")
    # dic_chapter = chapter.__dict__
    # json_str = json.dumps(dic_chapter)
    # print(json_str)

    """测试下载小说目录页面并形成小说对象"""
    # url = "http://www.23us.so/files/article/html/13/13639/index.html"
    #     # "http://www.23us.so/files/article/html/14/14117/index.html"
    # book_handler = BookDownLoadHandler()
    # book_info = book_handler.handler_menu_info(url)
    # print(book_info)
    # RabbitMqMessageHandler.sent_message(host="localhost",
    #                                     queue_name="book_download",
    #                                     routing_key_name="book_download",
    #                                     message_content=book_info)
    # print(book_info)

    """测试接收章节下载任务"""

    logging.basicConfig(level=logging.INFO,
                        format='%(levelname)s \t %(asctime)s ---------- \n\t%(message)s\n',
                        datefmt='%a, %d %b %Y %H:%M:%S',
                        filename=os.path.join(SAVE_DIR, 'log', 'ttt123.log'),
                        filemode='w')

    bookHandler = BookDownLoadHandler()
    RabbitMqMessageHandler.receive_message(host="localhost",
                                           queue_name="download_chapter",
                                           callback=bookHandler.receive_menu_download_callback)
    # RabbitMqMessageHandler.receive_message(host="localhost",
    #                                        queue_name="parse_menu_html",
    #                                        callback=bookHandler.receive_book_html_parse_callback)
