#! /usr/bin/env python3
# -*-coding:utf-8-*-
import datetime


class ChapterInfo:
    def __init__(self,
                 source_link,
                 chapter_title,
                 chapter_content, chapter_sortId=0, book_name=None):
        self.BookName = book_name
        self.ChapterSortId = chapter_sortId
        self.ChapterTitle = chapter_title
        self.SourceLink = source_link
        self.ChapterContent = chapter_content
        # self.UpdatedTime = datetime.now().strftime('%Y-%m-%d')


class MenuItemInfo():
    def __init__(self, url, title, sortId):
        self.Url = url
        self.SortId = sortId
        self.Title = title


class BookInfo:
    def __init__(self, book_name, menu_url, menu_list):
        self.BookName = book_name
        self.MenuUrl = menu_url
        self.MenuList = menu_list
