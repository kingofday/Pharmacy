import actionTypes from './actionTypes';

export function SendProductInoAction(product) {
    return {
        type: actionTypes.SENDPRODUCTINFO,
        payload: { ...product }
    };
};