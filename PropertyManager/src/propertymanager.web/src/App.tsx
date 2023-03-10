import logo from './logo.svg';
import './App.css';
import Main from './layouts/main'
import { createTheme } from './theme';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';

function App() {

    const theme = createTheme();

    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            

                <Main />
            
            
        </ThemeProvider>
  );
}

export default App;
