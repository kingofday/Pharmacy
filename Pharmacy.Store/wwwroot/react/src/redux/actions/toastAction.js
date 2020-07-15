import actionTypes from './actionTypes';

export function ShowToastAction(title, body) {
    return {
        type: actionTypes.SHOWTOAST,
        show: true,
        title,
        body
    };
};

export function CloseToastAction() {
    return {
        type: actionTypes.CLOSETOAST,
        show: false
    };
};