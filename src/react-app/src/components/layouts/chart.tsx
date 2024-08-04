type ChartLayoutProps = {
    children: React.ReactNode;
    title: string;
};

export const ChartLayout = ({ children, title }: ChartLayoutProps) => {
    return (
        <>
            {/*<Head title={title} />*/}
            <div className="py-6 h-full">
                <div className="w-full px-4 sm:px-6 md:px-8">
                    <h1 className="text-2xl font-semibold text-gray-900">{title}</h1>
                </div>
                <div className="h-full px-4 py-6 sm:px-6 md:px-8">
                    {children}
                </div>
            </div>
        </>
    );
};