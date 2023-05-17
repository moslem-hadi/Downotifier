import { configureStore } from '@reduxjs/toolkit';

import { authReducer } from './auth.slice';
import { jobApiCallReducer } from './jobApiCall.slice';
import { singleJobApiCallReducer } from './singleJobApiCall.slice';
import { jobApiCallUpsertReducer } from './jobApiCallUpsert.slice';
import { modalReducers } from './modal.slice';
import { loadingReducers } from './loading.slice';

export * from './auth.slice';
export * from './jobApiCall.slice';
export * from './singleJobApiCall.slice';
export * from './jobApiCallUpsert.slice';
export * from './modal.slice';
export * from './loading.slice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    jobApiCalls: jobApiCallReducer,
    modal: modalReducers,
    jobApiCallUpsert: jobApiCallUpsertReducer,
    jobApiCall: singleJobApiCallReducer,
    loading: loadingReducers,
  },
});
