import RoutesDefinition from "@/components/Navigation/RoutesDefinition";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import UserContextProvider from "./components/Context/UserContext";
const theme = createTheme({
  palette: {
    primary: {
      main: "#0052cc",
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
