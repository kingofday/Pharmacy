﻿const actionTypes = {
     //------------------------------------Auth Actions
    LOG_IN: 'LOG_IN',
    LOG_OUT: 'LOG_OUT',
    SIGN_UP:'SIGN_UP',
    SET_AUTH_NEXT_PAGE:'SET_AUTH_NEXT_PAGE',
    AUTH_GOTO_NEXT_PAGE:'AUTH_GOTO_NEXT_PAGE',
    SHOWMODAL: 'SHOWMODAL',
    CLOSEMODAL: 'CLOSEMODAL',
    //SHOWTOAST: 'SHOWTOAST',
    //CLOSETOAST: 'CLOSETOAST',
    //SENDPRODUCTINFO: 'SENDPRODUCTINFO',
    //------------------------------------Global Error
    SHOW_INIT_ERROR: 'SHOW_INIT_ERROR',
    Hide_INIT_ERROR: 'HIDE_INIT_ERROR',
    //------------------------------------Basket Actions
    ADD_TO_BASKET: 'ADD_TO_BASKET',
    REMOVE_FROM_BASKET: 'REMOVE_FROM_BASKET',
    UPDATE_BASKET: 'UPDATE_BASKET',
    CHANGED_BASKET_ITEMS: 'CHANGED_BASKET_ITEMS',
    CLEAR_BASKET: 'CLEAR_BASKET',
    SET_BASKET_ROUTE: 'SET_BASKET_ROUTE',
    CLEAR_TEMP_BASKET:'CLEAR_TEMP_BASKET',
    SET_BASKET_ID:'SET_BASKET_ID',
    SET_WHOLE:'SET_WHOLE',
    //------------------------------------Review Actions
    SET_ADDRESS: 'SET_ADDRESS',
    SET_LOCATION: 'SET_LOCATION',
    SET_DELIVERY_TYPE:'SET_DELIVERY_TYPE',
    //------------------------------------Products Filter
    SET_PRODUCTS_FILTER_NAME:'SET_PRODUCTS_FILTER_NAME',
    SET_PRODUCTS_FILTER_SORT:'SET_PRODUCTS_FILTER_SORT',
    SET_PRODUCTS_FILTER_CATEGORY:'SET_PRODUCTS_FILTER_CATEGORY',
    SET_PRODUCTS_FILTER_PRICE:'SET_PRODUCTS_FILTER_PRICE',
    SET_PRODUCTS_PAGE_NUMBER:'SET_PRODUCTS_PAGE_NUMBER',
    SET_PRODUCTS_MAXAVAILABLEPRICE:'SET_PRODUCTS_MAXAVAILABLEPRICE'
};
export default actionTypes;