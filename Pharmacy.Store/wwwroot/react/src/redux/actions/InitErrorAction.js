import actionTypes from './actionTypes';
import strings from './../../shared/constant';
export function ShowInitErrorAction(fetchData, message) {
    return {
        type: actionTypes.SHOW_INIT_ERROR,
        payload: {
            fetchData,
            message: message || strings.connectionFailed
        }
    };
};

export function HideInitErrorAction() {
    return {
        type: actionTypes.Hide_INIT_ERROR
    };
};


