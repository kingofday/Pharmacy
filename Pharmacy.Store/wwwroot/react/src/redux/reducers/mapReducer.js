import actionTypes from './../actions/actionTypes';
export default function modalReducer(state = { lat: null, lng: null }, action) {
    switch (action.type) {
        case actionTypes.SET_LOCATION:
            return { ...state, ...action.payload };
        default:
            return state;
    }
};