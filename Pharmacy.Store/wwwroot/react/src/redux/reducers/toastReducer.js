import actionTypes from './../actions/actionTypes';
export default function modalReducer(state = { show: false, title: '', body: '' }, action) {
    switch (action.type) {
        case actionTypes.SHOWTOAST:
            return { ...state, show: true, title: action.title, body: action.body };
        case actionTypes.CLOSETOAST:
            return { ...state, show: false };
        default:
            return { ...state };
    }
};