import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';

import { fetchWrapper } from '../_helpers';

// create slice

const name = 'jobApiCall';
const initialState = createInitialState();
const extraActions = createExtraActions();
const extraReducers = createExtraReducers();
const slice = createSlice({ name, initialState, extraReducers });

// exports

export const jobApiCallActions = { ...slice.actions, ...extraActions };
export const jobApiCallReducer = slice.reducer;

// implementation

function createInitialState() {
  return {};
}

function createExtraActions() {
  const baseUrl = `${process.env.REACT_APP_API_URL}jobApiCall`;

  return {
    getAll: getAll(),
    deleteItem: deleteItem(),
  };

  function getAll() {
    const pageSize = 5;
    return createAsyncThunk(
      `${name}/getAll`,
      async ({ page }, thunkApi) =>
        await fetchWrapper.get(`${baseUrl}?page=${page}&pageSize=${pageSize}`)
    );
  }
  function deleteItem() {
    //I wont ask to delete! I can use sweetalert.
    return createAsyncThunk(
      `${name}/deleteItem`,
      async ({ id }, thunkApi) => {
        await fetchWrapper.delete(`${baseUrl}/${id}`);
        return thunkApi.dispatch(jobApiCallActions.getAll({ page: 1 }));
      }
    );
  }
}

function createExtraReducers() {
  return {
    ...getAll(),
    ...deleteItem(),
  };

  function getAll() {
    var { pending, fulfilled, rejected } = extraActions.getAll;
    return {
      [pending]: state => {},
      [fulfilled]: (state, action) => {
        state.jobApiCalls = action.payload;
      },
      [rejected]: (state, action) => {
        state.jobApiCalls = { error: action.error };
      },
    };
  }
  function deleteItem() {
    var { pending, fulfilled, rejected } = extraActions.deleteItem;
    return {
      [pending]: state => {},
      [fulfilled]: (state, action) => {
        state.jobApiCalls = { ...state.jobApiCalls, deleted: true };
      },
      [rejected]: (state, action) => {
        state.jobApiCalls = { ...state.jobApiCalls, error: action.error };
      },
    };
  }
}
