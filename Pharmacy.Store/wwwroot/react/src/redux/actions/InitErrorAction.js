import actionTypes from './actionTypes';

export  function ShowInitErrorAction(fetchData, message) {
    return {
        type: actionTypes.SHOW_INIT_ERROR,
        payload: {
            fetchData,
            message
        }
    };
};

export  function HideInitErrorAction() {
    return {
        type: actionTypes.Hide_INIT_ERROR
    };
};


