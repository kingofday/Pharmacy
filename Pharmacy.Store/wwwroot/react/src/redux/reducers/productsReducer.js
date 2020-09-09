import actionTypes from '../actions/actionTypes';

const initState = {
    type: 0,
    name: '',
    pageNumber: 1,
    pageSize: 9,
    maxAvailablePrice: 0,
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
        case actionTypes.SET_PRODUCTS_FILTER_SORT:
            return { ...state, ...action.payload }
        case actionTypes.SET_PRODUCTS_PAGE_NUMBER:
            return { ...state, ...action.payload }
            case actionTypes.SET_PRODUCTS_FILTER_CATEGORY:
                console.log(action.payload);
                return { ...state, ...action.payload }
        case actionTypes.SET_PRODUCTS_MAXAVAILABLEPRICE:
            return { ...state, ...action.payload, maxPrice: state.maxPrice === 0 ? action.payload.maxAvailablePrice : state.maxPrice }
        default:
            return state;
    }
}