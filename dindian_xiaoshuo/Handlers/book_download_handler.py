#! /usr/bin/env python3
# -*-coding:utf-8-*-
import json
from bs4 import BeautifulSoup
from Dtos.book_model import BookInfo, MenuItemInfo, ChapterInfo
from Handlers.down_load_page import get_page_html
from rabbit_sender import RabbitMqMessageHandler


class BookDownLoadHandler:
    def __init__(self):
        pass

    def handler_received_menu_download_task(self, jsonBody):
        menu_list = json.loads(jsonBody, encoding="utf-8")

        for menu_item in menu_list:
            chapter_url = menu_item["Url"]
            content = self.__get_chaper_content(chapter_url)

            if len(content) == 0:
                continue

            chapter_info = ChapterInfo(
                url=chapter_url,
                chapter_id=menu_item["Id"],
                content=content,
                title=menu_item["Title"],
                sort_id=menu_item["SortId"])

            chapter_info_body = json.dumps(chapter_info.__dict__, default=lambda x: x.__dict__)

            RabbitMqMessageHandler.sent_message(host="localhost",
                                                queue_name="save_chapter",
                                                routing_key_name="save_chapter",
                                                message_content=chapter_info_body)
            print("sent message with chapter %d" % menu_item["Id"])


    def __get_chaper_content(self, url):
        """
        下载并解析小说页内容
        """
        chapter_content_html = get_page_html(url)

        if chapter_content_html is None:
            return ""

        soup = BeautifulSoup(chapter_content_html)
        chapter_content = soup.find(id='contents').text
        return chapter_content

    def handler_menu_info(self, menu_url):
        """获取已经转换成JSON格式的目录信息"""
        book_info = self.__get_menu_list(menu_url)
        json_str = json.dumps(book_info.__dict__, default=lambda x: x.__dict__)
        return json_str

    def __get_menu_list(self, menu_url):
        """获取目录列表信息集合"""
        menu_html = get_page_html(menu_url)
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
