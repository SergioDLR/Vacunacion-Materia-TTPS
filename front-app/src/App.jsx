import RoutesDefinition from "@/components/Navigation/RoutesDefinition";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import UserContextProvider from "./components/Context/UserContext";
const theme = createTheme({
  typography: {
    fontFamily: [
      "Encode Sans",
      "BlinkMacSystemFont",
      '"Segoe UI"',
      "Roboto",
      '"Helvetica Neue"',
      "Arial",
      "sans-serif",
      '"Apple Color Emoji"',
      '"Segoe UI Emoji"',
      '"Segoe UI Symbol"',
    ].join(","),
  },
  palette: {
    primary: {
      main: "#black",
      info: "#37BBED",
      textPrimary: "#0072bb",
    },
  },
  contained: {
    "&:hover": {
      backgroundColor: "white",
    },
  },
});
function App() {
  return (
    <ThemeProvider theme={theme}>
      <UserContextProvider>
        <RoutesDefinition />
      </UserContextProvider>
    </ThemeProvider>
  );
}

export default App;
