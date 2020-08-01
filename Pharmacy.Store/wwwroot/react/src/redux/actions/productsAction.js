import actionTypes from './actionTypes';

export function SetPageNumberAction(pageNumber) {
    return {
        type: actionTypes.SET_PRODUCTS_PAGE_NUMBER,
        payload: { pageNumber: pageNumber }
    };
};

export function SetNameAction(name) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_NAME,
        payload: { name: name }
    };
};

export function SetCategoryIdAction(id) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_CATEGORY,
        payload: { categoryId: id }
    };
};

export function SetSortTypeAction(sortType) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_SORT,
        payload: { type: sortType }
    };
};

export function SetMaxAvailablePriceAction(maxAvailablePrice) {
    return {
        type: actionTypes.SET_PRODUCTS_MAXAVAILABLEPRICE,
        payload: { maxAvailablePrice }
    };
};

export function SetPriceRangeAction(minPrice, maxPrice) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_PRICE,
        payload: { minPrice: minPrice, maxPrice: maxPrice }
    };
};