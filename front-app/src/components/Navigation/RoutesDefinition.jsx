import { Routes, Route } from "react-router-dom";
import Home from "../Home";
const RoutesDefinition = () => {
  return (
    <Routes>
      <Route path="*" element={<h1>Esta ruta no existe</h1>} />
      <Route path="/" element={<Home />} />
    </Routes>
  );
};

export default RoutesDefinition;
