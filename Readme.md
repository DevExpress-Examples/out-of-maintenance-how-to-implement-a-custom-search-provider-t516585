# How to: Implement a Custom Search Provider


This example demonstrates how to create a custom search provider.


<h3>Description</h3>

To do this, design a class that inherits&nbsp;the&nbsp;<a href="https://documentation.devexpress.com/#WPF/clsDevExpressXpfMapInformationDataProviderBasetopic">InformationDataProviderBase</a>&nbsp;class&nbsp;and implement the <strong>CreateData</strong>&nbsp;method in it. Then, design a class that inherits the&nbsp;<a href="https://documentation.devexpress.com/#wpf/clsDevExpressXpfMapIInformationDatatopic">IInformationData</a>&nbsp;interface and override its&nbsp;<a href="https://documentation.devexpress.com/#wpf/DevExpressXpfMapIInformationData_OnDataResponsetopic">OnDataResponse</a>&nbsp;event. Implement the&nbsp;<strong>Search</strong>&nbsp;method to provide custom search logic.

<br/>


