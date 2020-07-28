﻿const actionTypes = {
    LOGIN: 'LOGIN',
    LOGOUT: 'LOGOUT',
    SIGNUP: 'SIGNUP',
    SHOWMODAL: 'SHOWMODAL',
    CLOSEMODAL: 'CLOSEMODAL',
    SHOWTOAST: 'SHOWTOAST',
    CLOSETOAST: 'CLOSETOAST',
    SENDPRODUCTINFO: 'SENDPRODUCTINFO',
    SHOW_INIT_ERROR: 'SHOW_INIT_ERROR',
    Hide_INIT_ERROR: 'HIDE_INIT_ERROR',
    ADD_TO_BASKET: 'ADD_TO_BASKET',
    REMOVE_FROM_BASKET: 'REMOVE_FROM_BASKET',
    UPDATE_BASKET: 'UPDATE_BASKET',
    CHANGED_BASKET_ITEMS: 'CHANGED_BASKET_ITEMS',
    CLEAR_BASKET: 'CLEAR_BASKET',
    SET_ADDRESS: 'SET_ADDRESS',
    SET_LOCATION: 'SET_LOCATION',
    SET_BASKET_ROUTE: 'SET_BASKET_ROUTE',
    SET_WHOLE:'SET_WHOLE',
    SET_BASKET_ID:'SET_BASKET_ID',
    CLEAR_TEMP_BASKET:'CLEAR_TEMP_BASKET',
    //------------------------------------products filter
    SET_PRODUCTS_FILTER_NAME:'SET_PRODUCTS_FILTER_NAME',
    SET_PRODUCTS_FILTER_SORT:'SET_PRODUCTS_FILTER_SORT',
    SET_PRODUCTS_FILTER_CATEGORY:'SET_PRODUCTS_FILTER_CATEGORY',
    SET_PRODUCTS_FILTER_PRICE:'SET_PRODUCTS_FILTER_PRICE',
};
export default actionTypes;