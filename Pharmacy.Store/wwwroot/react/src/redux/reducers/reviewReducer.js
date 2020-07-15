import actionTypes from '../actions/actionTypes';

const initState = {
    address: null,
    cost: 0,
    reciever: '',
    mobileNumber: '',
    basketId: null
};

export default function reviewReducer(state = initState, action) {
    switch (action.type) {
        case actionTypes.SET_ADDRESS:
            return { ...state, ...action.payload }
        case actionTypes.SET_BASKET_ID:
            return { ...state, basketId: action.payload.basketId }
        default:
            return state;
    }
}