#! /usr/bin/env python3
# -*-coding:utf-8-*-
import json
import sys

import requests

from Dtos.book_model import ChapterInfo
from Handlers.book_download_handler import BookDownLoadHandler
from rabbit_sender import RabbitMqMessageHandler

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
    url = "http://www.23us.so/files/article/html/13/13639/index.html"
        # "http://www.23us.so/files/article/html/14/14117/index.html"
    book_handler = BookDownLoadHandler()
    book_info = book_handler.handler_menu_info(url)
    RabbitMqMessageHandler.sent_message(host="localhost",
                                        queue_name="book_download",
                                        routing_key_name="book_download",
                                        message_content=book_info)
    print(book_info)
