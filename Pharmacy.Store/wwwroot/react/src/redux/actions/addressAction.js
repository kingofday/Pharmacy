import actionTypes from './actionTypes';

export function SetAddrssAction(addr) {
    return {
        type: actionTypes.SET_ADDRESS,
        payload: { ...addr }
    };
};