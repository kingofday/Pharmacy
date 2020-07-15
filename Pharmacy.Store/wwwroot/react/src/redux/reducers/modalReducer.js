import actionTypes from './../actions/actionTypes';
export default function modalReducer(state = { show: false, title: '', body: null }, action) {
    switch (action.type) {
        case actionTypes.SHOWMODAL:
            return { ...state, show: true, title: action.title, body: action.body };
        case actionTypes.CLOSEMODAL:
            return { ...state, show: false };
        default:
            return { ...state };
    }
};