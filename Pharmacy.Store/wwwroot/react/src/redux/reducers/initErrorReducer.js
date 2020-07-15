import actionTypes from '../actions/actionTypes';

const initState = {
    show: false,
    message:'',
    fetchDatas: []
};

export default function initErrorReducer(state = initState, action) {
    switch (action.type) {
        case actionTypes.SHOW_INIT_ERROR:
            return { ...state, message: action.message, show: true, fetchDatas: [...state.fetchDatas, action.payload.fetchData] };
        case actionTypes.Hide_INIT_ERROR:
            return { ...state, show: false, fetchDatas: [] };
        default:
            return { ...state };
    }
};