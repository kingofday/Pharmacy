import actionTypes from '../actions/actionTypes';

const initState = {
    type: 0,
    minPrice: 0,
    maxPrice: 0,
    categoryId: null
};

export default function productsReducer(state = initState, action) {
    switch (action.type) {
        case actionTypes.SET_PRODUCTS_FILTER_NAME:
            return { ...state, ...action.payload }
        case actionTypes.SET_PRODUCTS_FILTER_PRICE:
            return { ...state, ...action.payload }
        case actionTypes.SET_PRODUCTS_FILTER_PRICE:
            return { ...state, ...action.payload }
        case actionTypes.SET_PRODUCTS_FILTER_SORT:
            return { ...state, ...action.payload }
        default:
            return state;
    }
}