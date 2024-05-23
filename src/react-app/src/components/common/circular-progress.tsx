import './circular-progress.css';

export const CircularProgress = ({ size = 50, strokeWidth = 4, progress = 50 }) => {
    const radius = (size - strokeWidth) / 2;
    const circumference = 2 * Math.PI * radius;
    const offset = circumference - (progress / 100) * circumference;

    return (
        <svg
            className="circular-progress"
            width={size}
            height={size}
        >
            <circle
                className="circular-progress-background"
                stroke="lightgray"
                strokeWidth={strokeWidth}
                fill="none"
                r={radius}
                cx={size / 2}
                cy={size / 2}
            />
            <circle
                className="circular-progress-bar"
                stroke="blue"
                strokeWidth={strokeWidth}
                strokeLinecap="round"
                fill="none"
                r={radius}
                cx={size / 2}
                cy={size / 2}
                style={{ strokeDasharray: circumference, strokeDashoffset: offset }}
            />
        </svg>
    );
};