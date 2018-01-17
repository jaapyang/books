#! /usr/bin/env python3
# -*-coding:utf-8-*-
import datetime


class ChapterInfo:
    def __init__(self, url, chapter_id, content, title="", sort_id=0):
        self.Url = url
        self.Id = chapter_id
        self.SortId = sort_id
        self.Title = title
        self.Content = content


class MenuItemInfo:
    def __init__(self, url, title, sortId):
        self.Url = url
        self.SortId = sortId
        self.Title = title


class BookInfo:
    def __init__(self, book_name, menu_url, menu_list):
        self.BookName = book_name
        self.MenuUrl = menu_url
        self.MenuList = menu_list
