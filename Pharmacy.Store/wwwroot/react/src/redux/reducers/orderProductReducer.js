import actionTypes from './../actions/actionTypes';
export default function modalReducer(state = { payload: { name: '', count: 1 } }, action) {
    switch (action.type) {
        case actionTypes.SENDPRODUCTINFO:
            return { ...state, payload: action.payload };
        default:
            return { ...state };
    }
};