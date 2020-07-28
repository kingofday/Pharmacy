import actionTypes from './actionTypes';

export function SetNameAction(q) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_NAME,
        payload: { name: q }
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

export function SetPriceRangeAction(minPrice, maxPrice) {
    return {
        type: actionTypes.SET_PRODUCTS_FILTER_PRICE,
        payload: { minPrice: minPrice, maxPrice: maxPrice }
    };
};