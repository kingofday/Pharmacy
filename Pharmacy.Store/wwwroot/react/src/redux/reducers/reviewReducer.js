import actionTypes from '../actions/actionTypes';

const initState = {
    address: null,
    delivery: null,
    comment: '',
    prescriptionId: null
};

export default function reviewReducer(state = initState, action) {
    switch (action.type) {
        case actionTypes.SET_ADDRESS:
            return { ...state, address: { ...action.payload } };
        case actionTypes.SET_DELIVERY_TYPE:
            return { ...state, delivery: { ...action.payload.delivery }, comment: action.payload.comment };
        case actionTypes.SET_PRESCRIPTION_ID:
            return { ...state, ...action.payload }
        case actionTypes.CLEAR_PRESCRIPTION_ID:
            return { ...state, prescriptionId: null }
        default:
            return state;
    }
}